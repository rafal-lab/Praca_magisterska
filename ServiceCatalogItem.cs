using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public class ServiceCatalogItem
    {
        public string Type { get; set; } //
        public string Catalog { get; set; } // 
        public string Name { get; set; } // 
        public string Version { get; set; } // 
        public int Count { get; set; }
        public string PaymentType { get; set; }
        public decimal TotalPrice { get; set; } // 
        

        // Constructor
        public ServiceCatalogItem(string type, string catalog, string name, string version, int count, decimal price, string paymentType)
        {
            Catalog = catalog;
            Name = name;
            Version = version;
            TotalPrice = price;
            Type = type;
            PaymentType = paymentType;
            Count = count;
        }
    }
}
