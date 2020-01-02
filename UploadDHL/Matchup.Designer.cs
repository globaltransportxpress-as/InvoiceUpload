namespace UploadDHL
{
    partial class Matchup
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
            this.XuMissingCalculation = new System.Windows.Forms.DataGridView();
            this.XuForwPart = new System.Windows.Forms.DataGridView();
            this.XuNonMatch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.XuMissingCalculation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuForwPart)).BeginInit();
            this.SuspendLayout();
            // 
            // XuMissingCalculation
            // 
            this.XuMissingCalculation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.XuMissingCalculation.Location = new System.Drawing.Point(12, 12);
            this.XuMissingCalculation.Name = "XuMissingCalculation";
            this.XuMissingCalculation.Size = new System.Drawing.Size(1420, 436);
            this.XuMissingCalculation.TabIndex = 0;
            this.XuMissingCalculation.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.XuMissingCalculation_CellClick);
            // 
            // XuForwPart
            // 
            this.XuForwPart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.XuForwPart.Location = new System.Drawing.Point(12, 489);
            this.XuForwPart.Name = "XuForwPart";
            this.XuForwPart.Size = new System.Drawing.Size(1420, 465);
            this.XuForwPart.TabIndex = 1;
            this.XuForwPart.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.XuForwPart_CellDoblClick);
            // 
            // XuNonMatch
            // 
            this.XuNonMatch.Location = new System.Drawing.Point(12, 960);
            this.XuNonMatch.Name = "XuNonMatch";
            this.XuNonMatch.Size = new System.Drawing.Size(115, 23);
            this.XuNonMatch.TabIndex = 2;
            this.XuNonMatch.Text = "Show non match";
            this.XuNonMatch.UseVisualStyleBackColor = true;
            this.XuNonMatch.Click += new System.EventHandler(this.XuNonMatch_Click);
            // 
            // Matchup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1459, 984);
            this.Controls.Add(this.XuNonMatch);
            this.Controls.Add(this.XuForwPart);
            this.Controls.Add(this.XuMissingCalculation);
            this.Name = "Matchup";
            this.Text = "Matchup";
            this.Load += new System.EventHandler(this.Matchup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.XuMissingCalculation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuForwPart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView XuMissingCalculation;
        private System.Windows.Forms.DataGridView XuForwPart;
        private System.Windows.Forms.Button XuNonMatch;
    }
}