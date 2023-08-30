using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static xxx.ServerRecommendation;
using static xxx.StartupType;
using LiveCharts;
using LiveCharts.WinForms;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

namespace xxx
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<ServiceCatalogItem> catalogItems = new List<ServiceCatalogItem>(); //kolecja atrybutów koncowych
                                                                                


        //data checking functions - starts
        public int checkInt(string value)
        {
            int value2 = 0;
            if (int.TryParse(value, out value2))
            {
                return  value2;
            }
            else
            {
                MessageBox.Show("Wybrana wartość nie jest liczbą.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return value2;
            }
        }
        public float checkFloat(string value)
        {
            float numOfEmployees = 0.0f;
            if (float.TryParse(value, out numOfEmployees))
            {
                return numOfEmployees;
            }
            else
            {
                MessageBox.Show("Wybrana wartość nie jest liczbą.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return numOfEmployees;
            }
        }
        public bool checkBool(string value)
        {
            if (value == "Tak")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int changeScale(string value)
        {
            if (value == "Bardzo ważne")
            {
                return 3;
            }
            else if (value == "Średnio ważne")
            {
                return 2;
            }
            else 
            {
                return 1;
            }
        }
        //data checking functions - end



        private async void calculateSecurityCost(int securityLevel)
        {
            //Azure Monitor w cloud/ on-prem Prometheus darmowy
            //Alert System Monitor
            //skuName = "Alerts"
            //string meterName= "Alerts System Log Monitored at 10 Minute Frequency"
            //string productName = "Azure Monitor"

            //skuName = "Basic Logs"
            //meterNae = ""Basic Logs Data Ingestion""


            //Azure Defender for Cosmos DB
            string serviceName = "Azure Defender";
            string ProductName = "Microsoft Defender for Azure Cosmos DB";
            string MeterName = "Standard 100 RU/s";
            AzureRetailPrice[] cloudDefenderCosmosPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1 hour and 100 RU/s
            var cloudMonthPriceDefenderCosmosAll = cloudDefenderCosmosPrices[0].RetailPrice * 730 * 4; //cena w dolarach

            //VPN Gateway
            serviceName = "VPN Gateway";
            ProductName = "VPN Gateway";
            MeterName = "VpnGw1";
            AzureRetailPrice[] cloudVPNPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1 hour
            var cloudMonthVPNAll = cloudVPNPrices[0].RetailPrice * 730; //cena w dolarach
            //w Onprem VPN jest darmowy trzeba aby skonfigurować na urzadzeniu

            //Key Vault
            serviceName = "Key Vault";
            ProductName = "Key Vault";
            MeterName = "Operations";
            AzureRetailPrice[] cloudKeyVaultPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 10K operations
            var cloudMonthKeyVaultAll = cloudKeyVaultPrices[0].RetailPrice * 100; //cena w dolarach

        } //security costs ---dokonczyc


        private async Task calculateStorageCost(float storageSize, int dataSecurityLevel, int scalabilityOptions)
        {
            //Azure Storage, przechowywanie plików itp.
            //Blob storage - unstructured data, such as images, videos, documents and backups
            //LRs- locally redundat storage 
            //ZRS - zone redundant storage. for application required high availability
            //string serviceName = "Storage"
            //skuName = "Hot LRS" / "Hot GRS"

            //Storage
            string serviceName = "Storage"; //storage account
            string storageDroductName = "General Block Blob v2";
            string storageMeterName = "Hot LRS Data Stored";
            AzureRetailPrice[] cloudStoragePrices = await AzureAsyncReadPriceForService(serviceName, storageMeterName, storageDroductName); //cost for 1MB
            var cloudMonthPriceForAllStorage = cloudStoragePrices[0].RetailPrice * 100* 100 * 2* storageSize; //cena w dolarach za Storage
            Logger.Log($"Storage calculation:");
            Logger.Log($"   Cloud:");
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {storageDroductName}, MeterName: {storageMeterName}, Cost: {cloudMonthPriceForAllStorage} zl");

            //write operations
            string writeOperationMeterName = "Hot LRS Write Operations";
            AzureRetailPrice[] cloudStoragePricesWrite = await AzureAsyncReadPriceForService(serviceName, writeOperationMeterName, storageDroductName); //cost for 10k of Writes
            var cloudMonthPriceForAllWrite = cloudStoragePricesWrite[0].RetailPrice * 500; //cena w dolarach za Storage
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {storageDroductName}, MeterName: {writeOperationMeterName}, Cost: {cloudMonthPriceForAllWrite} zl");

            //Console.WriteLine($"Miesieczna cena za przechowywanie w chmurze: {cloudMonthPriceForAllStorage} $");
            //Console.WriteLine($"Miesieczna cena za operację zapisu danych w chmurze: {cloudMonthPriceForAllWrite} $");
            double cloudAll = (cloudMonthPriceForAllStorage + cloudMonthPriceForAllWrite);
            decimal cloudAllDecimal = Convert.ToDecimal(cloudAll);
            Logger.Log($"       Sumarry cost: {cloudAllDecimal}");


            ServiceCatalogItem storageCloud = new ServiceCatalogItem("Cloud", "Storage", storageDroductName ,"Data Stored + Write Operations", 1, cloudAllDecimal, "month");
            AddItemToCatalogItems(storageCloud);

            //onprem
            SsdDiskAlgorithm algorithm = new SsdDiskAlgorithm();
            float requiredCapacityInTB = storageSize;

            SsdDiskConfiguration configuration = algorithm.FindSufficientDiskConfiguration(requiredCapacityInTB);
            decimal cenaDyskowSSD = 0;
            Logger.Log($"   OnPrem:");
            foreach (var kvp in configuration.DiskCountByCapacity)
            {
                //Console.WriteLine($"SSD Disk Capacity: {kvp.Key}GB, Quantity: {kvp.Value}");
                string amountGb = kvp.Key.ToString();
                DataTable nasSsdData = FetchDatabase("SsdStorage", "Capacity", amountGb);
                //var ssdDiskPrice = nasSsdData.AsEnumerable().FirstOrDefault();
                DataRow rowSSD = nasSsdData.Rows[0];
                decimal SsdPrice = Decimal.Parse(rowSSD["Price"].ToString());
                string SsdName = rowSSD["Model"].ToString();
                Logger.Log($"       SSD disk capacity: {amountGb}GB, Model: {SsdName}, Price: {SsdPrice} zl, Ilosc: {kvp.Value}");
                cenaDyskowSSD += SsdPrice;
            }
            int iloscDyskowSSD = configuration.DiskCountByCapacity.Count();
            Logger.Log($"       Summary Price: {cenaDyskowSSD} zl");
            
            //Data reading from DataBase
            DataTable nasDatabaseData = FetchDatabase("NasPricingTable", "Disks", "4");
            DataRow rowNasServer = nasDatabaseData.Rows[0];
            string serverName = rowNasServer["Model"].ToString();
            decimal NasPrice = Decimal.Parse(rowNasServer["Price"].ToString());
            Logger.Log($"       NAS server model: QNAP {serverName}, Price: {NasPrice} zl");
            ServiceCatalogItem storageOnPrem = new ServiceCatalogItem("OnPrem", "Storage", "QNAP NAS storage", $"NAS server {serverName} + SSD disks", 1, cenaDyskowSSD+NasPrice, "instantly");
            AddItemToCatalogItems(storageOnPrem);

        } //storage NAS/BlogStorge calculation
        private void calculateMaintenanceCost(int serverCount, string serverName, string procesorName, int FirewallhCount, int RouterCount, int SwitchCount, int scalability)
        {
            Logger.Log("Maintennance onprem.");
            //PRAD
            DataTable ServerTable = FetchDatabase("Server", "ServerName", serverName);
            DataRow rowServer = ServerTable.Rows[0];
            decimal ServerPower = Decimal.Parse(rowServer["Power"].ToString());
            
            DataTable processorTable = FetchDatabase("Procesors", "ProcesorName", procesorName);
            DataRow rowProcessor = processorTable.Rows[0];
            decimal ProcessorPower = Decimal.Parse(rowProcessor["Power"].ToString());

            DataTable pradTable = FetchDatabase("SerivcePricing", "ServiceName", "Prad");
            DataRow rowPrad= pradTable.Rows[0];
            decimal pradPrice = Decimal.Parse(rowPrad["MonthPrice"].ToString());
            decimal switchPowerUsage = 120.0m * SwitchCount;
            decimal routerPowerUsage = 120.0m * FirewallhCount;
            decimal firewallPowerUsage = 120.0m * RouterCount;
            decimal monthPradConsumption = ((ServerPower + ProcessorPower) * serverCount + switchPowerUsage + routerPowerUsage + firewallPowerUsage )* 24 * 30 / 1000.0m; //kWh
            decimal monthPradPrice = monthPradConsumption * pradPrice;
            //Console.WriteLine($"Zużycie pradu {monthPradConsumption}kWh, cena miesieczna {monthPradPrice}");
            Logger.Log($"    Service: Power, Provider: PGE, Plan: Small Companies, Consumption: {monthPradConsumption}kWh, Price: {monthPradPrice} zl");
            ServiceCatalogItem pradCloud = new ServiceCatalogItem("Cloud", "Power", "PGE", "Small Companies", 0, 0, "month");
            AddItemToCatalogItems(pradCloud);

            ServiceCatalogItem pradOnPrem = new ServiceCatalogItem("OnPrem", "Power", "PGE", "Small Companies", 1, monthPradPrice, "month");
            AddItemToCatalogItems(pradOnPrem);



            //UPS
            DataTable UPSTable = FetchDatabase("EastUpsPricing", "ModelPower", "800");
            DataRow rowUPS = UPSTable.Rows[0];
            string UPSName = rowUPS["ModeName"].ToString();
            decimal UpsPrice = decimal.Parse(rowUPS["ModelPrice"].ToString());
            decimal upsPower = 800.0m; // Moc jednego UPS-a w watach

            // Obliczamy ilość potrzebnych UPS-ów
            int numberOfUPS = (int)Math.Ceiling((ServerPower+ProcessorPower) *serverCount / upsPower);
            ServiceCatalogItem UpCLoud = new ServiceCatalogItem("Cloud", "UPS", "UPS", "not needed", 0, 0, "month");
            AddItemToCatalogItems(UpCLoud);

            ServiceCatalogItem UpsOnPrem = new ServiceCatalogItem("OnPrem", "UPS", UPSName, "UPS for Servers", numberOfUPS, UpsPrice*numberOfUPS, "instantly");
            Logger.Log($"    Service: UPS, Provider: EAST, Model: {UPSName}, Count: {numberOfUPS}, Total Price: {UpsPrice*numberOfUPS} zl");
            AddItemToCatalogItems(UpsOnPrem);

            //Cooling
            DataTable CoolingTable = FetchDatabase("RackCooling", "Name", "Wentylator");
            DataRow rowCooling = CoolingTable.Rows[0];
            string CoolingName = rowCooling["Model"].ToString();
            decimal CoolingPrice = decimal.Parse(rowCooling["Price"].ToString());
            ServiceCatalogItem CoolingCLoud = new ServiceCatalogItem("Cloud", "Cooling system", "-", "not needed", 0, 0, "month");
            AddItemToCatalogItems(CoolingCLoud);

            ServiceCatalogItem CoolingOnPrem = new ServiceCatalogItem("OnPrem", "Cooling system", CoolingName, "Cooling for RACK", 1, CoolingPrice, "instantly");
            Logger.Log($"    Service: RACK Cooling, Provider: RACK Sytems, Model: {CoolingName}, Count: 1, Total Price: {CoolingPrice} zl");
            AddItemToCatalogItems(CoolingOnPrem);


            //RACK
            int routerUnits = 1;
            int switchUnits = 1;
            int firewallUnits = 1;
            int UpsUnits = 2;
            int serverUnits = 1;
            int coolingUnits = 1;
            int redundancy = 0;
            if (scalability == 1)
            {
                redundancy =2;
            }
            else if (scalability == 2)
            {
                redundancy = 4;
            }
            else if (scalability == 3)
            {
                redundancy = 6;
            }
            decimal totalEquipmentUnits = routerUnits * RouterCount + switchUnits * SwitchCount + firewallUnits * FirewallhCount + serverUnits * serverCount + UpsUnits * numberOfUPS + coolingUnits * 1 + redundancy;
            int[] rackSizes = new int[] {9,15,18,24};
            Logger.Log($"       Total network equipment size: {totalEquipmentUnits}");
            int remainingUnits = (int)totalEquipmentUnits;
            List<int> usedRacks = new List<int>();

            while (remainingUnits > 0)
            {   
                if (remainingUnits >= rackSizes.Last())
                {
                    // Jeśli pozostało więcej jednostek do umieszczenia niż rozmiar szafy, użyj całej szafy
                    usedRacks.Add(rackSizes.Last());
                    remainingUnits -= rackSizes.Last();
                }
                else
                {
                    // Jeśli pozostało mniej jednostek niż rozmiar szafy, użyj najmniejszej szafy, która pomieści pozostałe jednostki
                    int smallestFittingRack = rackSizes.FirstOrDefault(rackSize => rackSize >= remainingUnits);
                    usedRacks.Add(smallestFittingRack);
                    remainingUnits = 0; // Wszystkie jednostki zostały umieszczone
                }                      
            }
            List<decimal> rackPrices = new List<decimal>();
            List<string> rackModels = new List<string>();

            foreach (int rack in usedRacks)
            {
                //Console.WriteLine($"RACK Size: {rack}");
                DataTable RackTable = FetchDatabase("RACK", "Size", rack.ToString());
                DataRow rowRack = RackTable.Rows[0];
                string RackName = rowRack["Model"].ToString();
                decimal RackPrice = decimal.Parse(rowRack["Price"].ToString());
                //Console.WriteLine($"RACK Model: {RackName} with price {RackPrice}");
                Logger.Log($"    Service: RACK, Provider: RACK, Model: {RackName}, Count: 1, Total Price: {RackPrice}  zl");
                rackPrices.Add(RackPrice);
                rackModels.Add(RackName);
            }
            decimal totalPrice = rackPrices.Sum();
            ServiceCatalogItem RackCLoud = new ServiceCatalogItem("Cloud", "RACKs", "-", "not needed", 0, 0, "month");
            AddItemToCatalogItems(RackCLoud);

            ServiceCatalogItem RackOnPrem = new ServiceCatalogItem("OnPrem", "RACKs", "RACK Systems", rackModels[0], rackModels.Count, totalPrice, "instantly");
            AddItemToCatalogItems(RackOnPrem);

        }
        public string GetSuggestedSolution(decimal startupBudget, decimal budgetPerMonth, decimal onPremBegginingPriceSum, decimal onPremMonthPriceSum, decimal cloudPriceSum)
        {
            if (startupBudget < onPremBegginingPriceSum && budgetPerMonth < cloudPriceSum)
            {
                return "Startup nie jest w stanie pozwolić sobie na żadną infrastrukturę. Skorzystaj z tańszego sprzętu.";
            }
            else if (startupBudget >= onPremBegginingPriceSum && budgetPerMonth < cloudPriceSum)
            {
                return "Infrastruktura OnPremise";
            }
            else if (startupBudget < onPremBegginingPriceSum && budgetPerMonth >= cloudPriceSum)
            {
                return " Infrastruktura Chmurowa";
            }
            else
            {
                decimal onPremTotalCost = onPremBegginingPriceSum + onPremMonthPriceSum * 12;
                decimal cloudTotalCost = cloudPriceSum * 12;
                //Console.WriteLine($"OnPrem total cost: {onPremTotalCost}");
                //Console.WriteLine($"Cloud total cost: {cloudTotalCost}");
                if (onPremTotalCost < cloudTotalCost)
                {
                    return "Infrastruktura OnPremise";
                }
                else
                {
                    return "Infrastruktura Chmurowa";
                }
            }
        }
        private void AddItemToCatalogItems(ServiceCatalogItem item)
        {
            catalogItems.Add(item);
        }//gotowe dodawanie do kolekcji
        decimal GetDevicePrice(string deviceName)
        {
            DataTable dataTable = FetchDatabase("UnifiPricingTable", "MachineName", deviceName);
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                decimal price = decimal.Parse((string)row["Price"]);
                //Console.WriteLine($"Cena netoworking: {price}");
                return price;
            }
            else
            {
                return 0; // Obsłuż sytuację, gdy urządzenie nie istnieje w bazie danych
            }
        }//gotowe
        private async Task<(int v1,int v2,int v3)> calculateNetworkingCost(int numberOfEmployees, int scalabilityLevel)
        {
            //Cloud -- start
            Logger.Log($"Networking calculation:");
            Logger.Log($"   Cloud:");
            
            //Azure Firewall
            string serviceName = "Azure Firewall";
            string ProductName = "Azure Firewall";
            string MeterName = "Basic Deployment";
            AzureRetailPrice[] cloudFirewallPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1 hour and 1 unit
            var cloudMonthPriceFirewall = cloudFirewallPrices[0].RetailPrice * 730 * 4; //cena w dolarach
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {ProductName}, MeterName: {MeterName}, Cost: {cloudMonthPriceFirewall} zl");
            //Azure Firewall Data Processed
            string MeterNameProcessing = "Basic Data Processed";
            AzureRetailPrice[] cloudProcessedFirewallPrices = await AzureAsyncReadPriceForService(serviceName, MeterNameProcessing, ProductName); //cost for 1GB
            var cloudMonthPriceFirewallProcessed = cloudProcessedFirewallPrices[0].RetailPrice * 50 * 4; //cena w dolarach
            double sumFirewall = cloudMonthPriceFirewallProcessed + cloudMonthPriceFirewall;
            decimal decSumFirewall = Convert.ToDecimal(sumFirewall);
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {ProductName}, MeterName: {MeterNameProcessing}, Cost: {cloudMonthPriceFirewallProcessed}  zl");
            Logger.Log($"       Summary Firewall cost: {decSumFirewall}");
            ServiceCatalogItem firewallCloud = new ServiceCatalogItem("Cloud", "Firewall", "AzureFirewall", "Firewall and DataProcessed", 1, decSumFirewall, "month");
            AddItemToCatalogItems(firewallCloud);


            //Virtual Router
            serviceName = "ExpressRoute";
            ProductName = "ExpressRoute Standard Gateway";
            MeterName = "Standard Gateway";
            AzureRetailPrice[] cloudRouterPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1 hour
            var cloudRouterPricesAll = cloudRouterPrices[0].RetailPrice * 730 * 4; //cena w dolarach
            decimal doubRouterPrices = Convert.ToDecimal(cloudRouterPricesAll);
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {ProductName}, MeterName: {MeterName}, Cost: {doubRouterPrices}  zl");
            ServiceCatalogItem routerCloud = new ServiceCatalogItem("Cloud", "Router", "ExpressRoute", "ExpressRoute Standard Gateway", 1, doubRouterPrices, "month");
            AddItemToCatalogItems(routerCloud);
            //Virtual Router

            //Load Balancer Data Processed
            serviceName = "Load Balancer";
            string Location = "Global";
            ProductName = "Load Balancer";
            MeterName = "Standard Data Processed";
            AzureRetailPrice[] cloudBalancerDataPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1GB
            var cloudBalancerPricesAll = cloudBalancerDataPrices[0].RetailPrice * 1000*4; //cena w dolarach
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {ProductName}, MeterName: {MeterName}, Cost: {cloudBalancerPricesAll}  zl");

            //LoadBalancer Work
            serviceName = "Load Balancer";
            Location = "Global";
            ProductName = "Load Balancer";
            MeterName = "Standard Included LB Rules and Outbound Rules";
            AzureRetailPrice[] cloudBalancerPrices = await AzureAsyncReadPriceForService(serviceName, MeterName, ProductName); //cost for 1GB
            var cloudBalancerPricesAll2 = cloudBalancerPrices[0].RetailPrice * 1000*4; //cena w dolarach
            double sumLoadBalancer = cloudBalancerPricesAll + cloudBalancerPricesAll2;
            decimal decSumBalancer = Convert.ToDecimal(sumLoadBalancer);
            Logger.Log($"       ServiceName: {serviceName}, ProductName: {ProductName}, MeterName: {MeterName}, Cost: {cloudBalancerPricesAll2}  zl");
            Logger.Log($"       Summary Load Balancer cost: {decSumBalancer}");
            ServiceCatalogItem balancerCloud = new ServiceCatalogItem("Cloud", "Load Balancer", "Azure Load Balance", "Data proccess + rules", 1, decSumBalancer, "month");
            AddItemToCatalogItems(balancerCloud);

            ServiceCatalogItem switchCloud = new ServiceCatalogItem("Cloud", "Switch", "Not needed", "Not needed", 0, 0, "month");
            AddItemToCatalogItems(switchCloud);
            //Cloud -- koniec


            //onprem -- start
            Logger.Log($"   OnPrem:");
            int usersPerAP = 250;
            int accessPointsCount = (numberOfEmployees + usersPerAP - 1) / usersPerAP; // Obliczenie liczby Access Pointów
            int freePortsOnSwitch;
            switch (scalabilityLevel)
            {
                case 3:
                    freePortsOnSwitch = 8;
                    break;
                case 2:
                    freePortsOnSwitch = 5;
                    break;
                case 1:
                    freePortsOnSwitch = 3;
                    break;
                default:
                    freePortsOnSwitch = 0;
                    break;
            }
            float switchCount = numberOfEmployees / (24f - 2f - freePortsOnSwitch);
            int roundedSwitchCount = (int)Math.Ceiling(switchCount); // Zaokrąglenie w górę do najbliższej jedności

            float routerCount = numberOfEmployees / 50f;
            int roundedRouterCount = (int)Math.Ceiling(routerCount); // Zaokrąglenie w górę do najbliższej jedności

            float firewallCount = numberOfEmployees / 50f;
            int roundedFirewallhCount = (int)Math.Ceiling(firewallCount); // Zaokrąglenie w górę do najbliższej jedności

            // Obliczamy cenę całkowitą dla każdego urządzenia na podstawie jego liczby oraz ceny z bazy danych
            decimal switchPrice = GetDevicePrice("UniFi Switch 24");
            decimal accessPointPrice = GetDevicePrice("UniFi AP AC Pro");
            decimal routerPrice = GetDevicePrice("Ubiquiti ER-12 EDGEMAX");
            decimal firewallPrice = GetDevicePrice("Juniper SRX300");

            decimal totalSwitchPrice = switchPrice * roundedSwitchCount;
            decimal totalAccessPointPrice = accessPointPrice * accessPointsCount;
            decimal totalRouterPrice = routerPrice * roundedRouterCount;
            decimal totalFirewallPrice = firewallPrice * roundedFirewallhCount;
            // Obliczamy sumaryczną cenę wszystkich urządzeń
            decimal totalPrice = totalSwitchPrice + totalAccessPointPrice + totalRouterPrice + totalFirewallPrice;


            ServiceCatalogItem switchOnPrem = new ServiceCatalogItem("OnPrem", "Switch", "Ubiqiti", "Switch 24", roundedSwitchCount, totalSwitchPrice, "instantly");
            Logger.Log($"       Switch model: Ubiquiti Switch 24, Number: {roundedSwitchCount}, Price All: {totalSwitchPrice} zl");
            AddItemToCatalogItems(switchOnPrem);

            ServiceCatalogItem routerOnPrem = new ServiceCatalogItem("OnPrem", "Router", "Ubiquiti", "ER-12 EDGEMAX", roundedRouterCount, totalRouterPrice, "instantly");
            Logger.Log($"       Router model: Ubiquiti ER-12 EDGEMAX, Number: {roundedRouterCount}, Price All: {totalRouterPrice} zl");
            AddItemToCatalogItems(routerOnPrem);

            ServiceCatalogItem firewallOnPrem = new ServiceCatalogItem("OnPrem", "Firewall", "Juniper", "SRX300", roundedFirewallhCount, totalFirewallPrice, "instantly");
            Logger.Log($"       Firewall model: Juniper SRX300, Number: {roundedFirewallhCount}, Price All: {totalFirewallPrice} zl");
            AddItemToCatalogItems(firewallOnPrem);

            ServiceCatalogItem accessPointOnPrem = new ServiceCatalogItem("OnPrem", "Access Point", "Ubiquiti", "AP AC Pro", accessPointsCount, totalAccessPointPrice, "instantly");
            Logger.Log($"       Access Point model: Ubiquiti AP AC Pro, Number: {accessPointsCount}, Price All: {totalAccessPointPrice} zl");
            AddItemToCatalogItems(accessPointOnPrem);

            ServiceCatalogItem accessPointCloud = new ServiceCatalogItem("Cloud", "Access Point", "Ubiquiti", "AP AC Pro", accessPointsCount, totalAccessPointPrice, "instantly");
            AddItemToCatalogItems(accessPointCloud);

            ServiceCatalogItem balancerOnPrem = new ServiceCatalogItem("OnPrem", "Load Balancer", "LB in router", "-", 0, 0, "instantly");
            Logger.Log($"       Load Balancer model: In router, Number: 0, Price All: 0 zl");
            AddItemToCatalogItems(balancerOnPrem);
            //onprem -- koniec

            return (roundedFirewallhCount, roundedRouterCount,roundedSwitchCount);
        }//gotowe urzadzenia sieciowe
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
        } //gotowe, read data from database
        public List<Service> getListOfStaticServicesWithPricesECommerce()
        {
            List<Service> services = new List<Service>();
            // Create a list of static services
            List<string> serviceNames = new List<string>
            {
                "Github",
                "Word Press",
                "WooComerce",
                "OptiMonk"
            };

            // Fetch data for each service using FetchDatabyService function
            foreach (string serviceName in serviceNames)
            {
                DataTable dataTable = FetchDatabase("SerivcePricing", "ServiceName", serviceName);

                // Create a new Service object
                Service service = new Service();
                service.ServiceName = serviceName;

                // Check if data was found for the service
                if (dataTable.Rows.Count > 0)
                {
                    // Extract the necessary columns from the DataTable
                    DataRow row = dataTable.Rows[0];
                    service.ServiceProvider = row["ServiceProvider"].ToString();
                    service.VersionName = row["VersionName"].ToString();
                    service.MonthPrice = row["MonthPrice"].ToString();
                    service.YearPrice = row["YearPrice"].ToString();
                    service.PricePer = row["PricePer"].ToString();
                }
                else
                {
                    // Set default values or handle the case when data is not found
                    service.ServiceProvider = "N/A";
                    service.VersionName = "N/A";
                    service.MonthPrice = "N/A";
                    service.YearPrice = "N/A";
                    service.PricePer = "N/A";
                }

                // Add the service to the list
                services.Add(service);
            }
            return services;
        } //gotowe, zmiany jak dochodzi nowe oprogramowanie
        public List<Service> getListOfStaticServicesWithPricesAiML()
        {
            List<Service> services = new List<Service>();
            // Create a list of static services
            List<string> serviceNames = new List<string>
            {
                "Github",
                "Visual Studio",
            };

            // Fetch data for each service using FetchDatabyService function
            foreach (string serviceName in serviceNames)
            {
                DataTable dataTable = FetchDatabase("SerivcePricing", "ServiceName", serviceName);

                // Create a new Service object
                Service service = new Service();
                service.ServiceName = serviceName;

                // Check if data was found for the service
                if (dataTable.Rows.Count > 0)
                {
                    // Extract the necessary columns from the DataTable
                    DataRow row = dataTable.Rows[0];
                    service.ServiceProvider = row["ServiceProvider"].ToString();
                    service.VersionName = row["VersionName"].ToString();
                    service.MonthPrice = row["MonthPrice"].ToString();
                    service.YearPrice = row["YearPrice"].ToString();
                    service.PricePer = row["PricePer"].ToString();
                }
                else
                {
                    // Set default values or handle the case when data is not found
                    service.ServiceProvider = "N/A";
                    service.VersionName = "N/A";
                    service.MonthPrice = "N/A";
                    service.YearPrice = "N/A";
                    service.PricePer = "N/A";
                }

                // Add the service to the list
                services.Add(service);
            }
            return services;
        } //gotowe, zmiany jak dochodzi nowe oprogramowanie
        public List<Service> getListOfStaticServicesWithPricesAnalysis()
        {
            List<Service> services = new List<Service>();
            // Create a list of static services
            List<string> serviceNames = new List<string>
            {
                "Github",
                "Visual Studio",
                "Power BI"
            };

            // Fetch data for each service using FetchDatabyService function
            foreach (string serviceName in serviceNames)
            {
                DataTable dataTable = FetchDatabase("SerivcePricing", "ServiceName", serviceName);

                // Create a new Service object
                Service service = new Service();
                service.ServiceName = serviceName;

                // Check if data was found for the service
                if (dataTable.Rows.Count > 0)
                {
                    // Extract the necessary columns from the DataTable
                    DataRow row = dataTable.Rows[0];
                    service.ServiceProvider = row["ServiceProvider"].ToString();
                    service.VersionName = row["VersionName"].ToString();
                    service.MonthPrice = row["MonthPrice"].ToString();
                    service.YearPrice = row["YearPrice"].ToString();
                    service.PricePer = row["PricePer"].ToString();
                }
                else
                {
                    // Set default values or handle the case when data is not found
                    service.ServiceProvider = "N/A";
                    service.VersionName = "N/A";
                    service.MonthPrice = "N/A";
                    service.YearPrice = "N/A";
                    service.PricePer = "N/A";
                }

                // Add the service to the list
                services.Add(service);
            }
            return services;
        } //gotowe, zmiany jak dochodzi nowe oprogramowanie
        private void calculateSoftwareCost(string startupType, string subscriptionType, int numberOfUsers)
        {
            List<Service> services = null;
            if (startupType == "AI/ Machine Learning")
            {
                services = getListOfStaticServicesWithPricesAiML();
            }
            else if (startupType == "ECommerce")
            {
                services = getListOfStaticServicesWithPricesECommerce();
            }
            else
            {
                services = getListOfStaticServicesWithPricesAnalysis();
            }
            decimal totalCost = 0;
            Logger.Log("Software calculation");
            Logger.Log("    General:");
            foreach (Service service in services)
            {
                decimal serviceCost = 0;

                if (subscriptionType == "Miesięczna")
                {
                    // Calculate the cost based on the type (Time or User)
                    if (service.PricePer.ToLower() == "time")
                    {
                        serviceCost = decimal.Parse(service.MonthPrice);
                    }
                    else if (service.PricePer.ToLower() == "user")
                    {
                        // Assuming the number of users is stored somewhere as numberOfUsers
                        serviceCost = decimal.Parse(service.MonthPrice) * numberOfUsers;
                    }

                }
                else
                {
                    // Calculate the cost based on the type (Time or User)
                    if (service.PricePer.ToLower() == "time")
                    {
                        serviceCost = decimal.Parse(service.YearPrice);
                    }
                    else if (service.PricePer.ToLower() == "user")
                    {
                        // Assuming the number of users is stored somewhere as numberOfUsers
                        serviceCost = decimal.Parse(service.YearPrice) * numberOfUsers;
                    }
                }


                // Print the service details and its cost
                Logger.Log($"        Service: {service.ServiceName}");
                Logger.Log($"        Provider: {service.ServiceProvider}");
                Logger.Log($"        Version: {service.VersionName}");
                Logger.Log($"        Monthly Cost: {serviceCost} zł");

                // Accumulate the total cost
                totalCost += serviceCost;
            }
            // Print the total cost for all services
            Logger.Log($"   Summary software cost for {startupType} startup: {totalCost} zł");
            ServiceCatalogItem switchSoftwareCost = new ServiceCatalogItem("OnPrem", "Software", "All software", "All software", 1, totalCost, "month");
            AddItemToCatalogItems(switchSoftwareCost);

            ServiceCatalogItem switchSoftwareCostCl = new ServiceCatalogItem("Cloud", "Software", "All software", "All software", 1, totalCost, "month");
            AddItemToCatalogItems(switchSoftwareCostCl);
        }//gotowe, obliczenia kosztów miesiecznych
        private async Task calculateDatabaseCost(string DatabaseType, string DatabaseSize)
        {
            float kursDolara = 4;
            Logger.Log($"Database calculation:");
            Logger.Log($"   Cloud:");
            if (DatabaseType == "NoSQL")
            {
                //Console.WriteLine("No SQL cost calculation:");
                //NoSQL database
                string serviceName = "Azure Cosmos DB";
                string databaseProductName = "Azure Cosmos DB";
                string databaseMeterName = "100 RU/s";
                //number of RU/s * hours * cost = final price
                AzureRetailPrice[] cloudDatabasePrices = await AzureAsyncReadPriceForService(serviceName, databaseMeterName, databaseProductName); //cost for 100RU/s
                var skuCounter = 4;
                var cloudMonthPriceUsage = kursDolara * skuCounter * 730 * cloudDatabasePrices[1].RetailPrice;
                Logger.Log($"       ServiceName: {serviceName}, ProductName: {databaseProductName}, MeterName: {databaseMeterName}, Cost: {cloudMonthPriceUsage}  zl");

                string serviceNameStorage = "Azure Cosmos DB";
                string databaseProductNameStorge = "Azure Cosmos DB";
                string databaseMeterNameStorage = "Data Stored";
                AzureRetailPrice[] cloudStoragePrices = await AzureAsyncReadPriceForService(serviceNameStorage, databaseProductNameStorge, databaseMeterNameStorage); //cost for 1GB
                var cloudMonthPriceStorage = kursDolara * float.Parse(DatabaseSize) * 0.25;
                Logger.Log($"       ServiceName: {serviceNameStorage}, ProductName: {databaseProductNameStorge}, MeterName: {databaseMeterNameStorage}, Cost: {cloudMonthPriceStorage}  zl");
                //Console.WriteLine($"Miesieczna cena za bazę NoSQL w chmurze: {cloudMonthPriceUsage} zl");
                //Console.WriteLine($"Miesieczna cena za przechowywanie danych w NoSQL w chmurze: {cloudMonthPriceStorage} zl");
                double sumNoSQL = cloudMonthPriceUsage + cloudMonthPriceStorage;
                decimal decSumNoSQL = Convert.ToDecimal(sumNoSQL);
                Logger.Log($"       Summary cost: {decSumNoSQL}");
                ServiceCatalogItem NoSQLCloud = new ServiceCatalogItem("Cloud", "Database", "Azure Cosmos DB", "400 RU/s", 1, decSumNoSQL, "month");
                AddItemToCatalogItems(NoSQLCloud);


            }
            else if (DatabaseType == "MS SQL")
            {
                Console.WriteLine("MS SQL cost calculation");
                //Azure SQL Database
                string serviceName = "SQL Database";
                string sqlDatabaseProductName = "SQL Database Single/Elastic Pool General Purpose - Compute Gen5"; // Hot LRS Blob Inventory Flatspace
                string sqlDatabaseSkuName = "2 vCore";
                AzureRetailPrice[] cloudStoragePrices = await AzureAsyncReadPriceForService(serviceName, sqlDatabaseSkuName, sqlDatabaseProductName); ; //cost for 1hour
                var pricePerHourOfUsage = cloudStoragePrices[0].RetailPrice; //DB cost
                Logger.Log($"       ServiceName: {serviceName}, ProductName: {sqlDatabaseProductName}, MeterName: {sqlDatabaseSkuName}, Cost: {pricePerHourOfUsage}  zl");

                string serviceName2 = "SQL Database";
                string sqlDatabaseLicense = "SQL Database Single/Elastic Pool General Purpose - SQL License"; // SQL license
                string sqlDatabaseSkuName2 = "vCore";
                AzureRetailPrice[] cloudStoragePricesLicense = await AzureAsyncReadPriceForService(serviceName2, sqlDatabaseSkuName2, sqlDatabaseLicense); ; //cost for 1hour
                var pricePerHourOfUsageLicense = cloudStoragePricesLicense[0].RetailPrice; //License cost
                var cloudMonthPriceForAllStorage = 730 * kursDolara * (pricePerHourOfUsage + pricePerHourOfUsageLicense); //cena za usługę i licencje
                Logger.Log($"       ServiceName: {serviceName2}, ProductName: {sqlDatabaseLicense}, MeterName: {sqlDatabaseSkuName2}, Cost: {cloudMonthPriceForAllStorage} zl");

                string serviceName3 = "SQL Database";
                string sqlDatabaseProductNameStorage = "SQL Database Single/Elastic Pool General Purpose - Storage"; // SQL license
                string sqlDatabaseSkuName3 = "General Purpose Data Stored";
                AzureRetailPrice[] cloudStoragePricesStorage = await AzureAsyncReadPriceForService(serviceName3, sqlDatabaseSkuName3, sqlDatabaseProductNameStorage); ; //cost for 1GB/Month
                var pricePerHourOfUsageStorage = kursDolara * float.Parse(DatabaseSize) * cloudStoragePricesStorage[0].RetailPrice; //cena za przechowywane dane
                Logger.Log($"       ServiceName: {serviceName3}, ProductName: {sqlDatabaseProductNameStorage}, MeterName: {sqlDatabaseSkuName3}, Cost: {cloudStoragePricesStorage} zl");


                Console.WriteLine($"Miesieczna cena za bazę MS SQL w chmurze: {cloudMonthPriceForAllStorage} zl");
                Console.WriteLine($"Cena za licencje MS SQL w chmurze to: {pricePerHourOfUsageStorage} zl");

                double sumSQL = pricePerHourOfUsageStorage + cloudMonthPriceForAllStorage + pricePerHourOfUsage;
                decimal decSumSQL = Convert.ToDecimal(sumSQL);
                Logger.Log($"       Summary cost: {decSumSQL} zl");
                ServiceCatalogItem NoSQLCloud = new ServiceCatalogItem("Cloud", "Database", "SQL Database", "Single database, storage + license", 1, decSumSQL, "month");
                AddItemToCatalogItems(NoSQLCloud);
            }

            Logger.Log(" Onprem:");
            DiskConfigurator diskConfigurator = new DiskConfigurator();
            
            //DataTable dataTable = FetchDatabase("SerivcePricing", "ServiceProvider", "CCIT");
            List<DiskConfiguration> selectedDisks = diskConfigurator.ConfigureDisksForDataSize(int.Parse(DatabaseSize));

            if (selectedDisks != null)
            {
                decimal totalPrice = 0;
                //Console.WriteLine("Wybrane dyski:");
                int DiskCount = selectedDisks.Count();
                foreach (var disk in selectedDisks)
                {
                    totalPrice += disk.Price;
                    Logger.Log($"       Model: {disk.Model}, Capacity: {disk.Capacity}GB, Price: {disk.Price} zł");
                }

                Logger.Log($"       Summary Price: {totalPrice} zł");
                ServiceCatalogItem SQLonPrem = new ServiceCatalogItem("OnPrem", "Database", "Database", "SSD database storage", DiskCount, totalPrice, "instantly");
                AddItemToCatalogItems(SQLonPrem);
            }
            else
            {
                Logger.Log("No sufficient config.");
            }


        }//gotowe, Database cost calculation
        private OutsourcingResult calculateInfManagementCost(string supportHours) //gotowe, cost for Infrastructure management
        {
            DataTable InfManagementCostTable = FetchDatabase("SerivcePricing", "ServiceProvider", "CCIT");
            DataRow[] rows = InfManagementCostTable.Select($"VersionName = 'Outsourcing IT {supportHours}'");

            OutsourcingResult outsourcingResult = new OutsourcingResult
            {
                MonthlyPrice = Convert.ToDouble(rows[0]["MonthPrice"]),
                CompanyName = "CCIT",
                SupportHours = supportHours,
            };
            decimal allCost = Convert.ToDecimal(outsourcingResult.MonthlyPrice);
            ServiceCatalogItem Management = new ServiceCatalogItem("Cloud", "Managemenet", "Infrastructure management", "Infrastructure managemen", 1, allCost, "month");
            AddItemToCatalogItems(Management);
            ServiceCatalogItem ManagementOnPRem = new ServiceCatalogItem("OnPrem", "Managemenet", "Infrastructure management", "Infrastructure managemen", 1, allCost, "month");
            AddItemToCatalogItems(ManagementOnPRem);
            return outsourcingResult;
        }
        private async Task<AzureRetailPrice[]> AzureAsyncReadPriceForService(string serviceName,  string meterName, string productName)
        {
            // Create an instance of AzureRetailPricesApiClient
            var apiClient = new AzureRetailPricesApiClient();
            try
            {
                Debug.WriteLine($"Started API query");
                // Get prices asynchronously
                List<AzureRetailPrice> allPrices = await apiClient.GetAllPricesAsync(serviceName,  meterName, productName);
                Debug.WriteLine($"Finished API query");
                Debug.WriteLine($"Prices found: {allPrices.Count}");
                // Filter prices for the desired service, operating system, and minimum RAM
                AzureRetailPrice[] filteredPrices = allPrices
                    .Where(price =>
                        price.ServiceName == serviceName &&
                        price.Type == "Consumption" 
 )
                    .ToArray();

                // Do something with the filtered prices
                foreach (var price in filteredPrices)
                {
                    Console.WriteLine($"Price: {price.UnitPrice} {price.CurrencyCode} per {price.UnitOfMeasure}, ServiceName {price.ServiceName}, Productname {price.ProductName}. Type: {price.Type}");
                }
                return filteredPrices;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to retrieve prices: {ex.Message}");
                return new AzureRetailPrice[0];
            }

        }//gotowe
        private async Task<VirtualServerResult> CalculateVirtualServer(string startupType, int stopienNadmiarowosci) //gotowe
        {
            //cloud
            //maszyny wirtualne
            //vCPU = (Threads x Cores) x Physcial CPU
            decimal kursDolara = 4;
            string serviceName = "Virtual Machines";
            string meterName = "";
            string productName = "";
            if (startupType == "AI/ Machine Learning")
            {
                meterName = "F8/F8s"; //AI/ML
                productName = "Virtual Machines F Series"; //compute optimized dla AI/ML
            }
            else if (startupType == "ECommerce")
            {
                productName = "Virtual Machines LS Series"; //storage optimized dla ECommerce
                meterName = "L4s"; //ECommerce //4vCPUs, 32GB RAM, 678 GB Temp stor
            }
            else
            {
                productName = "Virtual Machines Edv4 Series";//memory optimized dla Analysis Tools
                meterName = "E4d v4"; //Analysis 4vCPUs, 32GB RAM, 150GB Temp storage/ 0.288/hour
            }
            AzureRetailPrice[] cloudStoragePrices = await AzureAsyncReadPriceForService(serviceName, meterName, productName); //cost for 1MB
            var cloudMonthPriceForAllStorage = 700 * cloudStoragePrices[0].RetailPrice * 4;

            VirtualServerResult result = new VirtualServerResult
            {
                MonthlyPrice = cloudMonthPriceForAllStorage,
                ProductName = productName,
                MeterName = meterName
            };

            return result;
        }//Virtual Server

        private async void bOblicz_Click(object sender, EventArgs e)
        {
            catalogItems.Clear();
            // Read the user's input from the form
            CStartupInfo info = new CStartupInfo
            {
                StartupBudget = StartigBudgetTextBox.Text,
                CStartupType = StartupTypeComboBox.SelectedItem.ToString(),
                NumEmployees = checkInt(CurrentNumEmployeesTextBox.Text),
                CanManageOnPremises = checkBool(CanMangeOnPremComboBox.Text),
                DatabaseRequired = checkBool(databaseRequired.Text),
                DataSafety = changeScale(DataSecurityImportanceComboBox.Text),
                BudgetPerMonth = checkInt(monthBudgetTextBox.Text),
                ScalabilityOption = changeScale(ScalabilityImportanceComboBox.Text),
                DataStorage = checkFloat(RequiredMemoryTBTextBox.Text),
                softwareCostType = softwarePayment.Text,
                OutsourcingTime = godzinyWsparcia.Text,
            };
            if(info.DatabaseRequired == true)
            {
                info.DatabaseType = dBType.Text;
                info.DatabaseSize = textBoxDbSize.Text;
            }
            Logger.Log("The calculation was started.");
            Logger.Log("Input user data:.");
            Logger.Log($"   Content for Startup info:");
            Logger.Log($"   Startup type: {info.CStartupType}");
            Logger.Log($"   Starting budger: {info.StartupBudget}");
            Logger.Log($"   Month budget:  {info.BudgetPerMonth}");
            Logger.Log($"   Number of employees: {info.NumEmployees}");
            Logger.Log($"   Storage size [TB]: {info.DataStorage}");
            Logger.Log($"   Database required?: {info.DatabaseRequired}");
            Logger.Log($"   Database type: {info.DatabaseType}");
            Logger.Log($"   Database size: {info.DatabaseSize}");
            Logger.Log($"   Scalability: {info.ScalabilityOption}");
            Logger.Log($"   Security: {info.DataSafety}");
            Logger.Log($"   Infrastructure management: {info.CanManageOnPremises}");
            Logger.Log($"   Software payment method: {info.softwareCostType}");

            //storage/ pamiec masowa - start
            await calculateStorageCost(info.DataStorage, info.DataSafety, info.ScalabilityOption);
            //storage/ pamiec masowa -koniec


            //networking -- start
            var (FirewallhCount, RouterCount, SwitchCount) = await calculateNetworkingCost(info.NumEmployees, info.ScalabilityOption);
            //networking -- koniec


            //oprogramowanie -- start
            calculateSoftwareCost(info.CStartupType, info.softwareCostType, info.NumEmployees);
            //oprogramowanie --koniec


            //baza danych -- start
            if (info.DatabaseRequired == true)
            {
                await calculateDatabaseCost(info.DatabaseType, info.DatabaseSize);
            }
            //baza danych -- koniec


            //liczba pracowników --start
            int scale = (int)Math.Ceiling((double)info.NumEmployees / 30); //zaokrąglenie w górę
            //Console.WriteLine($"Liczba pracowników: {info.NumEmployees}");
            //Console.WriteLine($"Skala: {scale}");
            //liczba pracowików --koniec


            //serwery --start
            ServerRecommendation serverRec = new ServerRecommendation();

            // Parametry skalowalności i typu startupu
            int scalability = info.ScalabilityOption; // 1 - mała skalowalność, 2 - duża skalowalność
            string startupType = info.CStartupType; // Typ startupu: "AI/ML", "Ecommerce", "Analytics"

            // Zalecanie konfiguracji serwera na podstawie parametrów skalowalności i typu startupu
            ServerConfiguration recommendedServer = serverRec.RecommendServer(scalability, startupType);

            // Obliczanie całkowitego kosztu
            decimal totalCost = serverRec.CalculateTotalCost(recommendedServer);

            // Wyświetlanie zalecanej konfiguracji serwera i całkowitego kosztu
            Logger.Log("Server calculation:");
            Logger.Log("    OnPrem");
            Logger.Log("        Recomender physical server configuration:");
            Logger.Log("        Server name: " + recommendedServer.ServerName);
            Logger.Log("        Procesor: " + recommendedServer.ProcessorName);
            Logger.Log("        vCPUs: " + recommendedServer.vCPUs);
            Logger.Log("        RAM: " + recommendedServer.RAMSize + "GB");
            Logger.Log("        Memory: " + recommendedServer.StorageSize + "GB");
            Logger.Log("        Summary price: " + totalCost + " zł");
            Logger.Log($"       Number of servers: {scale}");

            ServiceCatalogItem ServerOnPrem = new ServiceCatalogItem("OnPrem", "Server", recommendedServer.ServerName, recommendedServer.ServerName, scale, totalCost*scale, "instantly");
            AddItemToCatalogItems(ServerOnPrem);

            Logger.Log("    Cloud:");
            VirtualServerResult costResult = await CalculateVirtualServer(info.CStartupType, info.ScalabilityOption);
            Logger.Log($"       ProductName: {costResult.ProductName}");
            Logger.Log($"       MeterName: {costResult.MeterName}");
            Logger.Log($"       Price for server: {costResult.MonthlyPrice} zl");
            Logger.Log($"       Number of servers: {scale}");
            decimal cloudServer = Convert.ToDecimal(costResult.MonthlyPrice);
            ServiceCatalogItem ServerCloud = new ServiceCatalogItem("Cloud", "Server", costResult.ProductName, costResult.MeterName , scale, cloudServer*scale, "month");
            AddItemToCatalogItems(ServerCloud);
            //serwery -- koniec

            //utrzymanie onprem prad/UPS itp poczatek
            calculateMaintenanceCost( scale, recommendedServer.ServerName, recommendedServer.ProcessorName, FirewallhCount, RouterCount, SwitchCount, info.ScalabilityOption);
            //utrzymanie - koniec


            //outsourcing --start
            if (info.CanManageOnPremises == false)
            {
                Logger.Log("Infrastructure Management");
                Logger.Log("    General");
                OutsourcingResult outsourcingResult = new OutsourcingResult();
                outsourcingResult = calculateInfManagementCost(info.OutsourcingTime);
                Logger.Log($"       CompanyName: {outsourcingResult.CompanyName}, HoursOfSupport {outsourcingResult.SupportHours}h ,Price: {outsourcingResult.MonthlyPrice} zl");

            }
            //outsourcing -- koniec



            //zebrane dane
            // Przykład wyświetlenia elementów z listy catalogItems
            // Pause the execution for 3 seconds (3000 milliseconds) using Task.Delay
            await Task.Delay(3000);
            Logger.Log("");
            Logger.Log("Generel calculated information");
            foreach (var item in catalogItems)
            {
                Logger.Log($"Type: {item.Type}, Catalog: {item.Catalog}, Name: {item.Name}, Version: {item.Version}, Count: {item.Count}, TotalPrice: {item.TotalPrice}, PaymentType: {item.PaymentType}");
            }
          
            //Final result
            // Display the total prices in a MessageBox
            decimal onPremBegginingPriceSum = catalogItems
                    .FindAll(item => item.Type == "OnPrem" && item.PaymentType == "instantly")
                    .Sum(item => item.TotalPrice);
            decimal onPremMonthPriceSum = catalogItems
                    .FindAll(item => item.Type == "OnPrem" && item.PaymentType == "month")
                    .Sum(item => item.TotalPrice);
            decimal cloudPriceSum = catalogItems
                    .FindAll(item => item.Type == "Cloud")
                    .Sum(item => item.TotalPrice);
            // Pobieramy budżety od użytkownika (info.StartupBudget i info.BudgetPerMonth)
            decimal startupBudget = Decimal.Parse(info.StartupBudget);
            string suggestedSolution = GetSuggestedSolution(startupBudget, info.BudgetPerMonth, onPremBegginingPriceSum, onPremMonthPriceSum, cloudPriceSum);
            
            string message = $"Solution after 12 months of working: {suggestedSolution:C}\n\n";
            message += $"Total (after 1 month) Cloud Price: {cloudPriceSum:C}\nTotal (after 1 month) OnPrem Price: {onPremBegginingPriceSum+ onPremMonthPriceSum:C}";
            await Task.Run(() =>
            {
                MessageBox.Show(message, "Suggested solution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
            Logger.Log($"{message}");
            FormChart chartForm = new FormChart(catalogItems);
            chartForm.Show();

            // Tworzymy nowe okno TableForm i przekazujemy listę catalogItems
            var tableForm = new TableForm(catalogItems);
            tableForm.Show();
            //azure pricing calculation
            try
            {

                //DNS configuration
                string serviceName = "Azure DNS";
                string dnsProductName = "Azure DNS";
                string dnsMeterName = "Public Zone";
               //await AzureAsyncReadPriceForService(serviceName, dnsMeterName, dnsProductName);

                //Virtual Network
                //string serviceName = "Virtual Network"
                //meterName = "Management NIC"
                //meterName = "Standard IPv4 Static Public IP"
                //meterName = "Inter-Region Egress"
                //meterName = "Inter-Region Ingress"
                //meterName = "Standard Data Processed - Ingress" //chyba to
                //armRegionName = "polandcentral"


                //DNS configuration
                //w on-prem gdy serwer Linux to DNS darmowy z pomocą BIND (Berkeley Internet Name Domain)
                //string armRegionName = "Zone 1"
                //string serviceName = "Azure DNS"
                //string skuName = "Private"
                //string meterName = "Private Zone" //pricing for every DNS Zone
                //string meterName = "Private Queries" //pricing for each milion of queries in Zones

                //router
                //string ServiceName = "Azure Route Server"
                //string location = "Global"


            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }




        private void databaseRequired_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(databaseRequired.Text == "Tak")
            {
                dBType.Visible = true;
                label7.Visible = true;
                textBoxDbSize.Visible = true;
                label11.Visible = true;
            }
            if (databaseRequired.Text == "Nie")
            {
                dBType.Visible = false;
                label7.Visible = false;
                textBoxDbSize.Visible = false;
                label11.Visible = false;
            }
        }

        private void textBoxDbSize_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxDbSize.Text = "";
        }

        private void RequiredMemoryTBTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            RequiredMemoryTBTextBox.Text = "";
        }

        private void CanMangeOnPremComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CanMangeOnPremComboBox.Text == "Nie")
            {
                godzinyWsparcia.Visible = true;
                label13.Visible = true;
            }
            if (CanMangeOnPremComboBox.Text == "Tak")
            {
                godzinyWsparcia.Visible = false;
                label13.Visible = false;
            }
        }
    }
}
