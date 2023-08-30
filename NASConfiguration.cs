using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public class NASConfiguration
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\raffr\\source\\repos\\xxx\\PricesDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        private DataTable FetchDatabase(string tableName, string key, string value)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = $"SELECT * FROM {tableName} WHERE {key} = @Value";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }

        private DataTable FilterNasServers(string rackSize)
        {
            return FetchDatabase("NasPricingTable", "RackSize", rackSize);
        }

        private DataTable FilterSsdStorage(int requiredCapacityInGB)
        {
            DataTable ssdStorage = FetchDatabase("SsdStorage", "Capacity", requiredCapacityInGB.ToString());

            if (ssdStorage.Rows.Count == 0)
            {
                // If there's no exact match, get all available SSD storage capacities.
                List<int> capacities = ssdStorage.AsEnumerable()
                    .Select(row => Convert.ToInt32(row["Capacity"]))
                    .ToList();

                List<List<int>> combinations = new List<List<int>>();
                List<int> currentCombination = new List<int>();
                FindCombinationSum(capacities, requiredCapacityInGB, 0, currentCombination, combinations);

                if (combinations.Count > 0)
                {
                    // Find the best combination (nearest to the required capacity).
                    int nearestCapacity = combinations
                        .OrderBy(combination => Math.Abs(combination.Sum() - requiredCapacityInGB))
                        .First()
                        .Sum();

                    ssdStorage = FetchDatabase("SsdStorage", "Capacity", nearestCapacity.ToString());
                }
                else
                {
                    // If no suitable SSD storage is available, you can handle this case as needed (e.g., show an error message).
                    // For now, we will just return an empty DataTable.
                    ssdStorage = new DataTable();
                }
            }

            return ssdStorage;
        }

        private void FindCombinationSum(List<int> capacities, int target, int startIndex, List<int> currentCombination, List<List<int>> combinations)
        {
            if (target == 0)
            {
                combinations.Add(new List<int>(currentCombination));
                return;
            }

            for (int i = startIndex; i < capacities.Count; i++)
            {
                int capacity = capacities[i];
                if (capacity <= target)
                {
                    currentCombination.Add(capacity);
                    FindCombinationSum(capacities, target - capacity, i, currentCombination, combinations);
                    currentCombination.RemoveAt(currentCombination.Count - 1);
                }
            }
        }

        public void CalculateConfiguration(int amountOfData, int secLevel)
        {
            int desiredCapacityInGB = amountOfData;

            if (secLevel == 3)
            {
                desiredCapacityInGB += 500;
            }
            else if (secLevel == 2)
            {
                desiredCapacityInGB += 250;
            }
            // For secLevel 1, the desired capacity remains the same.

            DataTable ssdStorage = FilterSsdStorage(desiredCapacityInGB);

            Console.WriteLine("\nRecommended SSD Storage:");
            //foreach (DataRow ssdRow in ssdStorage)
            //{
            //   Console.WriteLine($"- SSD: {ssdRow["Model"]} | Capacity: {ssdRow["Capacity"]} | Price: {ssdRow["Price"]}");
            //}
        }
    }
}
