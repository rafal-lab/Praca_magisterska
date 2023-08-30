using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xxx
{
    public class OnPremisesCostCalculator
    {
        // Constants for cost calculation
        private const decimal DellServerCostPerUnit = 5000m; // Average cost of a Dell server
        private const decimal UbiquitiNetworkingCostPerUnit = 1000m; // Average cost of Ubiquiti networking equipment
        // Method to calculate the cost of on-premises infrastructure
        public decimal CalculateCost(decimal totalBudget)
        {

            decimal hardwareCost = CalculateHardwareCost(totalBudget,2,2);
            decimal softwareCost = CalculateSoftwareCost(totalBudget);

            decimal totalCost = hardwareCost + softwareCost;

            return totalCost;
        }

        // Method to calculate the cost of hardware
        private decimal CalculateHardwareCost(decimal totalBudget, int numberOfDellServers, int numberOfUbiquitiNetworkingDevices)
        {
            decimal dellServerCost = DellServerCostPerUnit * numberOfDellServers;
            decimal ubiquitiNetworkingCost = UbiquitiNetworkingCostPerUnit * numberOfUbiquitiNetworkingDevices;
            decimal totalHardwareCost = dellServerCost + ubiquitiNetworkingCost;
            // Calculate the cost of hardware based on the allocated budget
            // Additional logic and calculations can be added based on your specific needs

            return totalHardwareCost;
        }

        // Method to calculate the cost of software
        private decimal CalculateSoftwareCost(decimal totalBudget)
        {
            decimal softwareBudget = totalBudget;

            // Calculate the cost of software based on the allocated budget
            // Additional logic and calculations can be added based on your specific needs

            return softwareBudget;
        }
    }
}
