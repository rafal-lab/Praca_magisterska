using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.WinForms;
using LiveCharts.Wpf;

namespace xxx
{
    public partial class FormChart : Form
    {
        private LiveCharts.WinForms.CartesianChart chart1;
        private List<ServiceCatalogItem> catalogItems;

        public FormChart(List<ServiceCatalogItem> items)
        {
            InitializeComponent();
            catalogItems = items;
            FormChart_Load(this,EventArgs.Empty);
        }

        private void FormChart_Load(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            // Create two lists to store the total cost for Cloud and OnPrem services for each month
            List<decimal> cloudCosts = new List<decimal>();
            List<decimal> onPremCosts = new List<decimal>();

            // Start from the current month
            DateTime currentDate = DateTime.Now;

            decimal onPremBegginingPriceSum = catalogItems
                    .FindAll(item => item.Type == "OnPrem" && item.PaymentType == "instantly")
                    .Sum(item => item.TotalPrice);
            decimal cloudBegginignPriceSum = catalogItems
                    .FindAll(item => item.Type == "Cloud")
                    .Sum(item => item.TotalPrice);
            cloudCosts.Add(cloudBegginignPriceSum);
            onPremCosts.Add(onPremBegginingPriceSum);
            // Calculate the total cost for each month over 24 months
            for (int i = 0; i < 23; i++)
            {
                // Get the total cost for Cloud and OnPrem services for the current month
                decimal cloudMonthlPriceSum = catalogItems
                    .FindAll(item => item.Type == "Cloud" && item.PaymentType == "month")
                    .Sum(item => item.TotalPrice);
                decimal onPremMonthPriceSum = catalogItems
                    .FindAll(item => item.Type == "OnPrem" && item.PaymentType == "month")
                    .Sum(item => item.TotalPrice);
                cloudBegginignPriceSum += cloudMonthlPriceSum;
                onPremBegginingPriceSum += onPremMonthPriceSum;
                // Add the total cost to the respective lists
                cloudCosts.Add(cloudBegginignPriceSum);
                onPremCosts.Add(onPremBegginingPriceSum);

                // Move to the next month
                currentDate = currentDate.AddMonths(1);
            }

            // Create a CartesianChart and add LineSeries to it
            chart1.Dock = DockStyle.Fill;
            Controls.Add(chart1);

            // Add LineSeries for Cloud and OnPrem data
            chart1.Series = new SeriesCollection
        {
            new LineSeries
            {
                Title = "Cloud",
                Values = new ChartValues<decimal>(cloudCosts),
            },
            new LineSeries
            {
                Title = "OnPrem",
                Values = new ChartValues<decimal>(onPremCosts),

            }

        };
            chart1.LegendLocation = LegendLocation.Bottom;

            List<string> numbersAsString = new List<string>();
            for (int i = 1; i <= 24; i++)
            {
                numbersAsString.Add(i.ToString());
            }
            // Set labels for the X axis (months)
            chart1.AxisX.Add(new Axis
            {
                Title = "Months",
                Labels = numbersAsString
            });

            // Set title and labels for the Y axis (total cost)
            chart1.AxisY.Add(new Axis
            {
                Title = "Total Cost",
                LabelFormatter = value => value.ToString("C")
            });
        }

        private void InitializeComponent()
        {
            this.chart1 = new LiveCharts.WinForms.CartesianChart();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(932, 633);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "cartesianChart1";
            // 
            // FormChart
            // 
            this.ClientSize = new System.Drawing.Size(974, 721);
            this.Controls.Add(this.chart1);
            this.Name = "FormChart";
            this.ResumeLayout(false);

        }
    }
}
