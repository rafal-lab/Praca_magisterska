using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    
    public enum DataSafety
    {
        Low,
        Medium,
        High,
        Very_high
    }
    public class CStartupInfo
    {
        public string CStartupType { get; set; }
        public decimal InfrastructureCost { get; set; }
        public int NumEmployees { get; set; }
        public int BudgetPerMonth { get; set; }
        public bool CanManageOnPremises { get; set; }
        public bool DatabaseRequired { get; set; }
        public string DatabaseType { get; set; } = string.Empty;
        public string DatabaseSize { get; set; } = string.Empty;
        public string softwareCostType { get; set; }
        public string StartupBudget { get; set; }
        public int DataSafety { get; set; }
        public int ScalabilityOption { get; set; }
        public float DataStorage { get; set; }
        public string OutsourcingTime { get; set; }

    }

}
