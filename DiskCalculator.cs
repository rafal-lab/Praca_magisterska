using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{

    public class SsdDisk
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
    }

    public class SsdDiskConfiguration
    {
        public Dictionary<int, int> DiskCountByCapacity { get; set; }

        public SsdDiskConfiguration()
        {
            DiskCountByCapacity = new Dictionary<int, int>();
        }
    }

    public class SsdDiskAlgorithm
    {
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\raffr\\source\\repos\\xxx\\PricesDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        public SsdDiskConfiguration FindSufficientDiskConfiguration(float requiredCapacityInTB)
        {
            SsdDiskConfiguration configuration = new SsdDiskConfiguration();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT [Id], [Capacity] FROM [dbo].[SsdStorage]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<SsdDisk> disks = new List<SsdDisk>();

                        while (reader.Read())
                        {
                            SsdDisk disk = new SsdDisk
                            {
                                Id = reader.GetInt32(0),
                                Capacity = (int)reader.GetDouble(1)
                            };

                            disks.Add(disk);
                        }

                        disks.Sort((a, b) => b.Capacity.CompareTo(a.Capacity));

                        float remainingCapacity = requiredCapacityInTB * 1024; // Convert TB to GB

                        foreach (SsdDisk disk in disks)
                        {
                            int diskCapacity = disk.Capacity;
                            int diskCount = (int)(remainingCapacity / diskCapacity);

                            if (diskCount > 0)
                            {
                                configuration.DiskCountByCapacity.Add(diskCapacity, diskCount);
                                remainingCapacity -= diskCount * diskCapacity;
                            }
                        }
                    }
                }
            }

            return configuration;
        }
    }

}