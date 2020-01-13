namespace UploadDHL
{
    partial class Dialog
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
            this.XuAssignPrice = new System.Windows.Forms.Button();
            this.XuReaclOil = new System.Windows.Forms.CheckBox();
            this.XuNewPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.XuMakeOK = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // XuAssignPrice
            // 
            this.XuAssignPrice.Location = new System.Drawing.Point(152, 77);
            this.XuAssignPrice.Name = "XuAssignPrice";
            this.XuAssignPrice.Size = new System.Drawing.Size(75, 23);
            this.XuAssignPrice.TabIndex = 0;
            this.XuAssignPrice.Text = "Assign";
            this.XuAssignPrice.UseVisualStyleBackColor = true;
            this.XuAssignPrice.Click += new System.EventHandler(this.XuAssignPrice_Click);
            // 
            // XuReaclOil
            // 
            this.XuReaclOil.AutoSize = true;
            this.XuReaclOil.Location = new System.Drawing.Point(29, 54);
            this.XuReaclOil.Name = "XuReaclOil";
            this.XuReaclOil.Size = new System.Drawing.Size(67, 17);
            this.XuReaclOil.TabIndex = 1;
            this.XuReaclOil.Text = "Recal oil";
            this.XuReaclOil.UseVisualStyleBackColor = true;
            this.XuReaclOil.CheckedChanged += new System.EventHandler(this.XuReaclOil_CheckedChanged);
            // 
            // XuNewPrice
            // 
            this.XuNewPrice.Location = new System.Drawing.Point(29, 77);
            this.XuNewPrice.Name = "XuNewPrice";
            this.XuNewPrice.Size = new System.Drawing.Size(100, 20);
            this.XuNewPrice.TabIndex = 2;
            this.XuNewPrice.TextChanged += new System.EventHandler(this.XuNewPrice_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Add value to selected rows  price ";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // XuMakeOK
            // 
            this.XuMakeOK.AutoSize = true;
            this.XuMakeOK.Location = new System.Drawing.Point(29, 32);
            this.XuMakeOK.Name = "XuMakeOK";
            this.XuMakeOK.Size = new System.Drawing.Size(172, 17);
            this.XuMakeOK.TabIndex = 4;
            this.XuMakeOK.Text = "Records OK (on recalc oil also)";
            this.XuMakeOK.UseVisualStyleBackColor = true;
            this.XuMakeOK.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 129);
            this.Controls.Add(this.XuMakeOK);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.XuNewPrice);
            this.Controls.Add(this.XuReaclOil);
            this.Controls.Add(this.XuAssignPrice);
            this.Name = "Dialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dialog";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.Dialog_Shown);
            this.Leave += new System.EventHandler(this.Dialog_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button XuAssignPrice;
        private System.Windows.Forms.CheckBox XuReaclOil;
        private System.Windows.Forms.TextBox XuNewPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox XuMakeOK;
    }
}