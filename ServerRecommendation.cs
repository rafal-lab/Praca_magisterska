using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public class ServerRecommendation
    {
        public struct ServerConfiguration
        {
            public string ServerName;
            public string ProcessorName;
            public int vCPUs;
            public int RAMSize;
            public string RAMModel;
            public int StorageSize;
            public string StorageModel;
            public int Power;
            public decimal Price;
        }
        private DataTable FetchDatabase(string DatabaseName, string key, string Value)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\raffr\\source\\repos\\xxx\\PricesDatabase.mdf;Integrated Security=True;Connect Timeout=30"; // Replace with your actual database connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = $"SELECT * FROM {DatabaseName} WHERE {key} = @Value";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Value", Value);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        } //read data from database

        public ServerConfiguration RecommendServer(int scalability, string startupType)
        {
            string serverName = (scalability == 1) ? "DELL PowerEdge R350" : "DELL PowerEdge R450";
            string processorName;
            int ramSize;
            int storageSize;

            if (startupType == "AI/ Machine Learning")
            {
                processorName = (scalability == 1) ? "Intel® Xeon® E-2334" : "Intel® Xeon® Silver 4314";
                ramSize = 32;
                storageSize = 500;
            }
            else if (startupType == "ECommerce")
            {
                processorName = (scalability == 1) ? "Intel® Xeon® E-2314" : "Intel® Xeon® Silver 4309Y";
                ramSize = 32;
                storageSize = 1000;
            }
            else if (startupType == "Analysis/ Analytics tools")
            {
                processorName = (scalability == 1) ? "Intel® Xeon® E-2324G" : "Intel® Xeon® Silver 4309Y";
                ramSize = 64;
                storageSize = 250;
            }
            else
            {
                throw new ArgumentException("Nieprawidłowy typ startupu.");
            }

            // Wykonaj zapytanie do bazy danych, aby pobrać odpowiednie informacje
            DataTable processorData = FetchDatabase("dbo.Procesors", "ServerName", serverName);
            DataRow processorRow = processorData.AsEnumerable()
                .FirstOrDefault(row => row.Field<string>("ProcesorName") == processorName);

            DataTable ramData = FetchDatabase("dbo.RAM", "SerwerName", serverName);
            DataRow ramRow = ramData.AsEnumerable()
                .FirstOrDefault(row => row.Field<int>("Size") == ramSize);

            DataTable storageData = FetchDatabase("dbo.SsdStorage", "Capacity", storageSize.ToString());
            DataRow storageRow = storageData.Rows[0];

            DataTable serwerData = FetchDatabase("dbo.Server", "ServerName", serverName.ToString());
            DataRow serwerRow = serwerData.Rows[0];

            // Tworzenie zalecanej konfiguracji serwera na podstawie odczytanych danych
            ServerConfiguration recommendedConfig = new ServerConfiguration
            {
                ServerName = serverName,
                ProcessorName = processorRow["ProcesorName"].ToString(),
                vCPUs = Convert.ToInt32(processorRow["vCPUs"]),
                RAMSize = Convert.ToInt32(ramRow["Size"]),
                RAMModel = (ramRow["RamName"]).ToString(),
                StorageSize = Convert.ToInt32(storageRow["Capacity"]),
                StorageModel = (storageRow["Model"]).ToString(),
                Power = Convert.ToInt32(processorRow["Power"]) + Convert.ToInt32(serwerRow["Power"]),
                Price = Convert.ToDecimal(storageRow["Price"]) + Convert.ToDecimal(serwerRow["Price"]) + 
                        Convert.ToDecimal(processorRow["Price"]) + Convert.ToDecimal(ramRow["Price"]),
            };

            return recommendedConfig;
        }

        // Metoda do obliczania całkowitego kosztu serwera
        public decimal CalculateTotalCost(ServerConfiguration serverConfig)
        {
            return serverConfig.Price;
        }
    }
}
