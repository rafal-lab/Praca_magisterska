using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xxx
{
    public partial class TableForm : Form
    {
        private List<ServiceCatalogItem> catalogItems;

        public TableForm(List<ServiceCatalogItem> items)
        {
            InitializeComponent();
            catalogItems = items;
            TableForm_Load(this, EventArgs.Empty);
        }

        private void TableForm_Load(object sender, EventArgs e)
        {
            // Sortujemy elementy listy alfabetycznie po polu "Catalog"
            catalogItems = catalogItems.OrderBy(item => item.Catalog).ToList();

            // Separate the data for Cloud and OnPrem types
            var cloudItems = catalogItems.Where(item => item.Type == "Cloud").ToList();
            var onPremItems = catalogItems.Where(item => item.Type == "OnPrem").ToList();

            // Ustawiamy źródło danych dla DataGridViewCloud
            dataGridViewCloud.DataSource = cloudItems;

            // Ustawiamy źródło danych dla DataGridViewOnPrem
            dataGridViewOnPrem.DataSource = onPremItems;

            // Ukrywamy niepotrzebne kolumny (jeśli trzeba)
            dataGridViewCloud.Columns["Type"].Visible = false; // Ukrywamy kolumnę "Type" dla DataGridViewCloud, jeśli chcesz
            dataGridViewOnPrem.Columns["Type"].Visible = false; // Ukrywamy kolumnę "Type" dla DataGridViewOnPrem, jeśli chcesz

            // Ustawiamy nagłówki kolumn
            dataGridViewCloud.Columns["Catalog"].HeaderText = "Catalog";
            dataGridViewCloud.Columns["Name"].HeaderText = "Name";
            dataGridViewCloud.Columns["Version"].HeaderText = "Version";
            dataGridViewCloud.Columns["Count"].HeaderText = "Count";
            dataGridViewCloud.Columns["TotalPrice"].HeaderText = "Total Price";
            dataGridViewCloud.Columns["PaymentType"].HeaderText = "Payment Type";


            dataGridViewOnPrem.Columns["Catalog"].HeaderText = "Catalog";
            dataGridViewOnPrem.Columns["Name"].HeaderText = "Name";
            dataGridViewOnPrem.Columns["Version"].HeaderText = "Version";
            dataGridViewOnPrem.Columns["Count"].HeaderText = "Count";
            dataGridViewOnPrem.Columns["TotalPrice"].HeaderText = "Total Price";
            dataGridViewOnPrem.Columns["PaymentType"].HeaderText = "Payment Type";


            // Ustawiamy tytuły dla tabel
            labelCloud.Text = "Cloud Services";
            labelOnPrem.Text = "OnPrem Services";

        }

    }
}
