using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public class VirtualServerResult
    {
        public double MonthlyPrice { get; set; }
        public string ProductName { get; set; }
        public string MeterName { get; set; }
    }

    public class OutsourcingResult
    {
        public double MonthlyPrice { get; set; }
        public string CompanyName { get; set; }
        public string SupportHours { get; set; }
    }

    public class DiskConfiguration
    {
        public string Model { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
    }

    public class DiskConfigurator
    {
        private DataTable FetchDatabase(string DatabaseName)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\raffr\\source\\repos\\xxx\\PricesDatabase.mdf;Integrated Security=True;Connect Timeout=30"; // Replace with your actual database connection string

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = $"SELECT * FROM {DatabaseName}";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        } //read data from database
        public List<DiskConfiguration> ConfigureDisksForDataSize(int dataSize)
        {
            DataTable dataTable = FetchDatabase("SsdStorage");
            if (dataTable == null || dataTable.Rows.Count == 0)
            {
                // Brak dostępnych dysków w bazie danych
                return null;
            }

            // Konwertowanie danych z DataTable na listę obiektów DiskConfiguration
            List<DiskConfiguration> configurations = new List<DiskConfiguration>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                configurations.Add(new DiskConfiguration
                {
                    Model = dataTable.Rows[i]["Model"].ToString(),
                    Capacity = Convert.ToInt32(dataTable.Rows[i]["Capacity"]),
                    Price = Convert.ToDecimal(dataTable.Rows[i]["Price"])
                });
            }

            // Sortowanie dysków według pojemności (malejąco)
            configurations = configurations.OrderByDescending(disk => disk.Capacity).ToList();

            // Wyszukiwanie odpowiednich dysków do konfiguracji
            List<DiskConfiguration> selectedDisks = new List<DiskConfiguration>();
            int remainingData = dataSize;

            foreach (var disk in configurations)
            {
                if (remainingData <= 0)
                    break;

                if (disk.Capacity <= remainingData)
                {
                    selectedDisks.Add(disk);
                    remainingData -= disk.Capacity;
                }
            }

            if (remainingData > 0)
            {
                selectedDisks.Add(configurations.Last());
            }

            return selectedDisks;
        }
    }


}
