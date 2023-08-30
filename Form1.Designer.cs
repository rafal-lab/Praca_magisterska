using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace xxx
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
       /* protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.Blue, Color.Red, 45))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }*/
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartupTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartigBudgetTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CurrentNumEmployeesTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CanMangeOnPremComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DataSecurityImportanceComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.monthBudgetTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ScalabilityImportanceComboBox = new System.Windows.Forms.ComboBox();
            this.RequiredMemoryTBTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.bOblicz = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.databaseRequired = new System.Windows.Forms.ComboBox();
            this.dBType = new System.Windows.Forms.ComboBox();
            this.textBoxDbSize = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.softwarePayment = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.godzinyWsparcia = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // StartupTypeComboBox
            // 
            this.StartupTypeComboBox.FormattingEnabled = true;
            this.StartupTypeComboBox.Items.AddRange(new object[] {
            "AI/ Machine Learning",
            "ECommerce",
            "Analysis/ Analytics tools"});
            this.StartupTypeComboBox.Location = new System.Drawing.Point(12, 28);
            this.StartupTypeComboBox.Name = "StartupTypeComboBox";
            this.StartupTypeComboBox.Size = new System.Drawing.Size(215, 24);
            this.StartupTypeComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Wybierz czym zajmuje się twój startup:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(432, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Podaj początkowy budżet jaki jest w stanie przeznaczyć na infrastrukturę:";
            // 
            // StartigBudgetTextBox
            // 
            this.StartigBudgetTextBox.Location = new System.Drawing.Point(12, 105);
            this.StartigBudgetTextBox.Name = "StartigBudgetTextBox";
            this.StartigBudgetTextBox.Size = new System.Drawing.Size(100, 22);
            this.StartigBudgetTextBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(301, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Podaj aktualną liczbę zatrudnionych pracowników";
            // 
            // CurrentNumEmployeesTextBox
            // 
            this.CurrentNumEmployeesTextBox.Location = new System.Drawing.Point(15, 239);
            this.CurrentNumEmployeesTextBox.Name = "CurrentNumEmployeesTextBox";
            this.CurrentNumEmployeesTextBox.Size = new System.Drawing.Size(100, 22);
            this.CurrentNumEmployeesTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 605);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "Czy posiadasz więdzę do zarządzania infrastrukturą?";
            // 
            // CanMangeOnPremComboBox
            // 
            this.CanMangeOnPremComboBox.FormattingEnabled = true;
            this.CanMangeOnPremComboBox.Items.AddRange(new object[] {
            "Tak",
            "Nie"});
            this.CanMangeOnPremComboBox.Location = new System.Drawing.Point(9, 628);
            this.CanMangeOnPremComboBox.Name = "CanMangeOnPremComboBox";
            this.CanMangeOnPremComboBox.Size = new System.Drawing.Size(121, 24);
            this.CanMangeOnPremComboBox.TabIndex = 8;
            this.CanMangeOnPremComboBox.SelectedIndexChanged += new System.EventHandler(this.CanMangeOnPremComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 530);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(548, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Jak ważne jest dla ciebie bezpieczeństwo danych oraz możliwości pełnej kontroli n" +
    "ad nimi?";
            // 
            // DataSecurityImportanceComboBox
            // 
            this.DataSecurityImportanceComboBox.FormattingEnabled = true;
            this.DataSecurityImportanceComboBox.Items.AddRange(new object[] {
            "Bardzo ważne",
            "Średnio ważne",
            "Mało ważne"});
            this.DataSecurityImportanceComboBox.Location = new System.Drawing.Point(12, 558);
            this.DataSecurityImportanceComboBox.Name = "DataSecurityImportanceComboBox";
            this.DataSecurityImportanceComboBox.Size = new System.Drawing.Size(121, 24);
            this.DataSecurityImportanceComboBox.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 140);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(298, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "Jaki masz miesięczny budżet na infrastrukturę IT?";
            // 
            // monthBudgetTextBox
            // 
            this.monthBudgetTextBox.Location = new System.Drawing.Point(15, 169);
            this.monthBudgetTextBox.Name = "monthBudgetTextBox";
            this.monthBudgetTextBox.Size = new System.Drawing.Size(100, 22);
            this.monthBudgetTextBox.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 453);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(373, 16);
            this.label9.TabIndex = 17;
            this.label9.Text = "Jak ważne są dla ciebie możliwości skalowania infrastruktury?";
            // 
            // ScalabilityImportanceComboBox
            // 
            this.ScalabilityImportanceComboBox.FormattingEnabled = true;
            this.ScalabilityImportanceComboBox.Items.AddRange(new object[] {
            "Bardzo ważne",
            "Średnio ważne",
            "Mało ważne"});
            this.ScalabilityImportanceComboBox.Location = new System.Drawing.Point(12, 482);
            this.ScalabilityImportanceComboBox.Name = "ScalabilityImportanceComboBox";
            this.ScalabilityImportanceComboBox.Size = new System.Drawing.Size(121, 24);
            this.ScalabilityImportanceComboBox.TabIndex = 18;
            // 
            // RequiredMemoryTBTextBox
            // 
            this.RequiredMemoryTBTextBox.Location = new System.Drawing.Point(12, 309);
            this.RequiredMemoryTBTextBox.Name = "RequiredMemoryTBTextBox";
            this.RequiredMemoryTBTextBox.Size = new System.Drawing.Size(146, 22);
            this.RequiredMemoryTBTextBox.TabIndex = 20;
            this.RequiredMemoryTBTextBox.Text = "TB";
            this.RequiredMemoryTBTextBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RequiredMemoryTBTextBox_MouseClick);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 280);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(477, 16);
            this.label10.TabIndex = 19;
            this.label10.Text = "Jak dużo danych (plików/video/baza danych) zamierzasz przechowywać?";
            // 
            // bOblicz
            // 
            this.bOblicz.AutoEllipsis = true;
            this.bOblicz.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.bOblicz.Location = new System.Drawing.Point(288, 753);
            this.bOblicz.Name = "bOblicz";
            this.bOblicz.Size = new System.Drawing.Size(208, 72);
            this.bOblicz.TabIndex = 21;
            this.bOblicz.Text = "Oblicz";
            this.bOblicz.UseVisualStyleBackColor = false;
            this.bOblicz.Click += new System.EventHandler(this.bOblicz_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 366);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(259, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "Czy potrzebujesz bazy danych?";
            // 
            // databaseRequired
            // 
            this.databaseRequired.FormattingEnabled = true;
            this.databaseRequired.Items.AddRange(new object[] {
            "Tak",
            "Nie"});
            this.databaseRequired.Location = new System.Drawing.Point(9, 399);
            this.databaseRequired.Name = "databaseRequired";
            this.databaseRequired.Size = new System.Drawing.Size(121, 24);
            this.databaseRequired.TabIndex = 23;
            this.databaseRequired.SelectedIndexChanged += new System.EventHandler(this.databaseRequired_SelectedIndexChanged);
            // 
            // dBType
            // 
            this.dBType.FormattingEnabled = true;
            this.dBType.Items.AddRange(new object[] {
            "NoSQL",
            "MS SQL"});
            this.dBType.Location = new System.Drawing.Point(320, 399);
            this.dBType.Name = "dBType";
            this.dBType.Size = new System.Drawing.Size(121, 24);
            this.dBType.TabIndex = 24;
            this.dBType.Visible = false;
            // 
            // textBoxDbSize
            // 
            this.textBoxDbSize.Location = new System.Drawing.Point(526, 399);
            this.textBoxDbSize.Name = "textBoxDbSize";
            this.textBoxDbSize.Size = new System.Drawing.Size(162, 22);
            this.textBoxDbSize.TabIndex = 26;
            this.textBoxDbSize.Text = "GB";
            this.textBoxDbSize.Visible = false;
            this.textBoxDbSize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBoxDbSize_MouseClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(523, 366);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(184, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Ilośc danych w bazie danych?";
            this.label7.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(317, 366);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(117, 16);
            this.label11.TabIndex = 27;
            this.label11.Text = "Typ bazy danych?";
            this.label11.Visible = false;
            // 
            // softwarePayment
            // 
            this.softwarePayment.FormattingEnabled = true;
            this.softwarePayment.Items.AddRange(new object[] {
            "Miesięczna",
            "Roczna"});
            this.softwarePayment.Location = new System.Drawing.Point(9, 695);
            this.softwarePayment.Name = "softwarePayment";
            this.softwarePayment.Size = new System.Drawing.Size(323, 24);
            this.softwarePayment.TabIndex = 29;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 667);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(211, 16);
            this.label12.TabIndex = 28;
            this.label12.Text = "Forma opłat za oprogramowanie?";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(523, 605);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(216, 16);
            this.label13.TabIndex = 31;
            this.label13.Text = "Ilość godzin wsparcia firmy zewnętrznej";
            this.label13.Visible = false;
            // 
            // godzinyWsparcia
            // 
            this.godzinyWsparcia.FormattingEnabled = true;
            this.godzinyWsparcia.Items.AddRange(new object[] {
            "5",
            "10",
            "20"});
            this.godzinyWsparcia.Location = new System.Drawing.Point(526, 638);
            this.godzinyWsparcia.Name = "godzinyWsparcia";
            this.godzinyWsparcia.Size = new System.Drawing.Size(121, 24);
            this.godzinyWsparcia.TabIndex = 30;
            this.godzinyWsparcia.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(1053, 837);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.godzinyWsparcia);
            this.Controls.Add(this.softwarePayment);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxDbSize);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dBType);
            this.Controls.Add(this.databaseRequired);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bOblicz);
            this.Controls.Add(this.RequiredMemoryTBTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.ScalabilityImportanceComboBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.monthBudgetTextBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.DataSecurityImportanceComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CanMangeOnPremComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CurrentNumEmployeesTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.StartigBudgetTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartupTypeComboBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox StartupTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox StartigBudgetTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CurrentNumEmployeesTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CanMangeOnPremComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox DataSecurityImportanceComboBox;
        private Label label8;
        private TextBox monthBudgetTextBox;
        private Label label9;
        private ComboBox ScalabilityImportanceComboBox;
        private TextBox RequiredMemoryTBTextBox;
        private Label label10;
        private Button bOblicz;
        private Label label6;
        private ComboBox databaseRequired;
        private ComboBox dBType;
        private TextBox textBoxDbSize;
        private Label label7;
        private Label label11;
        private ComboBox softwarePayment;
        private Label label12;
        private Label label13;
        private ComboBox godzinyWsparcia;
    }
}

