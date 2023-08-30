namespace xxx
{
    partial class TableForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewCloud = new System.Windows.Forms.DataGridView();
            this.dataGridViewOnPrem = new System.Windows.Forms.DataGridView();
            this.labelCloud = new System.Windows.Forms.Label();
            this.labelOnPrem = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCloud)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOnPrem)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCloud
            // 
            this.dataGridViewCloud.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCloud.Location = new System.Drawing.Point(0, 74);
            this.dataGridViewCloud.Name = "dataGridViewCloud";
            this.dataGridViewCloud.ReadOnly = true;
            this.dataGridViewCloud.RowHeadersWidth = 51;
            this.dataGridViewCloud.RowTemplate.Height = 24;
            this.dataGridViewCloud.Size = new System.Drawing.Size(882, 450);
            this.dataGridViewCloud.TabIndex = 0;
            // 
            // dataGridViewOnPrem
            // 
            this.dataGridViewOnPrem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOnPrem.Location = new System.Drawing.Point(888, 74);
            this.dataGridViewOnPrem.Name = "dataGridViewOnPrem";
            this.dataGridViewOnPrem.ReadOnly = true;
            this.dataGridViewOnPrem.RowHeadersWidth = 51;
            this.dataGridViewOnPrem.RowTemplate.Height = 24;
            this.dataGridViewOnPrem.Size = new System.Drawing.Size(885, 450);
            this.dataGridViewOnPrem.TabIndex = 1;
            // 
            // labelCloud
            // 
            this.labelCloud.AutoSize = true;
            this.labelCloud.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.labelCloud.Location = new System.Drawing.Point(349, 24);
            this.labelCloud.Name = "labelCloud";
            this.labelCloud.Size = new System.Drawing.Size(86, 31);
            this.labelCloud.TabIndex = 2;
            this.labelCloud.Text = "label1";
            // 
            // labelOnPrem
            // 
            this.labelOnPrem.AutoSize = true;
            this.labelOnPrem.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.labelOnPrem.Location = new System.Drawing.Point(1296, 24);
            this.labelOnPrem.Name = "labelOnPrem";
            this.labelOnPrem.Size = new System.Drawing.Size(86, 31);
            this.labelOnPrem.TabIndex = 3;
            this.labelOnPrem.Text = "label2";
            // 
            // TableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1785, 700);
            this.Controls.Add(this.labelOnPrem);
            this.Controls.Add(this.labelCloud);
            this.Controls.Add(this.dataGridViewOnPrem);
            this.Controls.Add(this.dataGridViewCloud);
            this.Name = "TableForm";
            this.Text = "TableForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCloud)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOnPrem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCloud;
        private System.Windows.Forms.DataGridView dataGridViewOnPrem;
        private System.Windows.Forms.Label labelCloud;
        private System.Windows.Forms.Label labelOnPrem;
    }
}