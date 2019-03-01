namespace UploadDHL
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.XuDHL = new System.Windows.Forms.Button();
            this.Xu_EditDHL = new System.Windows.Forms.Button();
            this.Message = new System.Windows.Forms.Label();
            this.FileDone = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.XuMsgGrid = new System.Windows.Forms.DataGridView();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filenameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jumpLinesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.XuEditTranslation = new System.Windows.Forms.Button();
            this.XuSaveTranslatioon = new System.Windows.Forms.Button();
            this.XuEditTranslationGrid = new System.Windows.Forms.DataGridView();
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXTranspDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXProductDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.translationRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.XuFedex = new System.Windows.Forms.Button();
            this.XuEditTransFedex = new System.Windows.Forms.Button();
            this.XuEditTransGLS = new System.Windows.Forms.Button();
            this.XuGls = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.XuMsgGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuEditTranslationGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.translationRecordBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // XuDHL
            // 
            this.XuDHL.Location = new System.Drawing.Point(22, 86);
            this.XuDHL.Name = "XuDHL";
            this.XuDHL.Size = new System.Drawing.Size(165, 23);
            this.XuDHL.TabIndex = 0;
            this.XuDHL.Text = "DHL";
            this.XuDHL.UseVisualStyleBackColor = true;
            this.XuDHL.Click += new System.EventHandler(this.XuDHL_Click);
            // 
            // Xu_EditDHL
            // 
            this.Xu_EditDHL.Location = new System.Drawing.Point(217, 86);
            this.Xu_EditDHL.Name = "Xu_EditDHL";
            this.Xu_EditDHL.Size = new System.Drawing.Size(150, 23);
            this.Xu_EditDHL.TabIndex = 1;
            this.Xu_EditDHL.Text = "Edit Translation";
            this.Xu_EditDHL.UseVisualStyleBackColor = true;
            this.Xu_EditDHL.Click += new System.EventHandler(this.Xu_EditDHL_Click);
            // 
            // Message
            // 
            this.Message.AutoSize = true;
            this.Message.Location = new System.Drawing.Point(68, 591);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(0, 13);
            this.Message.TabIndex = 2;
            // 
            // FileDone
            // 
            this.FileDone.AutoSize = true;
            this.FileDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileDone.Location = new System.Drawing.Point(27, 12);
            this.FileDone.Name = "FileDone";
            this.FileDone.Size = new System.Drawing.Size(74, 20);
            this.FileDone.TabIndex = 3;
            this.FileDone.Text = "File done";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(22, 149);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(165, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "POSTNORD";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // XuMsgGrid
            // 
            this.XuMsgGrid.AllowUserToAddRows = false;
            this.XuMsgGrid.AllowUserToDeleteRows = false;
            this.XuMsgGrid.AutoGenerateColumns = false;
            this.XuMsgGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.XuMsgGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.XuMsgGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.statusDataGridViewTextBoxColumn,
            this.filenameDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn,
            this.jumpLinesDataGridViewTextBoxColumn});
            this.XuMsgGrid.DataSource = this.gridDataBindingSource;
            this.XuMsgGrid.Location = new System.Drawing.Point(432, 12);
            this.XuMsgGrid.Name = "XuMsgGrid";
            this.XuMsgGrid.Size = new System.Drawing.Size(658, 553);
            this.XuMsgGrid.TabIndex = 6;
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            this.statusDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusDataGridViewTextBoxColumn.Width = 62;
            // 
            // filenameDataGridViewTextBoxColumn
            // 
            this.filenameDataGridViewTextBoxColumn.DataPropertyName = "Filename";
            this.filenameDataGridViewTextBoxColumn.HeaderText = "Filename";
            this.filenameDataGridViewTextBoxColumn.Name = "filenameDataGridViewTextBoxColumn";
            this.filenameDataGridViewTextBoxColumn.Width = 74;
            // 
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            this.commentDataGridViewTextBoxColumn.ReadOnly = true;
            this.commentDataGridViewTextBoxColumn.Width = 76;
            // 
            // jumpLinesDataGridViewTextBoxColumn
            // 
            this.jumpLinesDataGridViewTextBoxColumn.DataPropertyName = "JumpLines";
            this.jumpLinesDataGridViewTextBoxColumn.HeaderText = "JumpLines";
            this.jumpLinesDataGridViewTextBoxColumn.Name = "jumpLinesDataGridViewTextBoxColumn";
            this.jumpLinesDataGridViewTextBoxColumn.ReadOnly = true;
            this.jumpLinesDataGridViewTextBoxColumn.Width = 82;
            // 
            // gridDataBindingSource
            // 
            this.gridDataBindingSource.DataSource = typeof(UploadDHL.GridData);
            // 
            // XuEditTranslation
            // 
            this.XuEditTranslation.Location = new System.Drawing.Point(217, 150);
            this.XuEditTranslation.Name = "XuEditTranslation";
            this.XuEditTranslation.Size = new System.Drawing.Size(150, 23);
            this.XuEditTranslation.TabIndex = 7;
            this.XuEditTranslation.Text = "Edit Translation";
            this.XuEditTranslation.UseVisualStyleBackColor = true;
            this.XuEditTranslation.Click += new System.EventHandler(this.XuEditTranslation_Click);
            // 
            // XuSaveTranslatioon
            // 
            this.XuSaveTranslatioon.Location = new System.Drawing.Point(432, 572);
            this.XuSaveTranslatioon.Name = "XuSaveTranslatioon";
            this.XuSaveTranslatioon.Size = new System.Drawing.Size(197, 32);
            this.XuSaveTranslatioon.TabIndex = 8;
            this.XuSaveTranslatioon.Text = "Save ";
            this.XuSaveTranslatioon.UseVisualStyleBackColor = true;
            this.XuSaveTranslatioon.Click += new System.EventHandler(this.XuSaveTranslatioon_Click);
            // 
            // XuEditTranslationGrid
            // 
            this.XuEditTranslationGrid.AllowUserToAddRows = false;
            this.XuEditTranslationGrid.AllowUserToDeleteRows = false;
            this.XuEditTranslationGrid.AutoGenerateColumns = false;
            this.XuEditTranslationGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.XuEditTranslationGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.XuEditTranslationGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.keyDataGridViewTextBoxColumn,
            this.keyTypeDataGridViewTextBoxColumn,
            this.gTXNameDataGridViewTextBoxColumn,
            this.gTXTranspDataGridViewTextBoxColumn,
            this.gTXProductDataGridViewTextBoxColumn});
            this.XuEditTranslationGrid.DataSource = this.translationRecordBindingSource;
            this.XuEditTranslationGrid.Location = new System.Drawing.Point(432, 31);
            this.XuEditTranslationGrid.Name = "XuEditTranslationGrid";
            this.XuEditTranslationGrid.Size = new System.Drawing.Size(658, 534);
            this.XuEditTranslationGrid.TabIndex = 9;
            // 
            // keyDataGridViewTextBoxColumn
            // 
            this.keyDataGridViewTextBoxColumn.DataPropertyName = "Key";
            this.keyDataGridViewTextBoxColumn.HeaderText = "Key";
            this.keyDataGridViewTextBoxColumn.Name = "keyDataGridViewTextBoxColumn";
            this.keyDataGridViewTextBoxColumn.ReadOnly = true;
            this.keyDataGridViewTextBoxColumn.Width = 50;
            // 
            // keyTypeDataGridViewTextBoxColumn
            // 
            this.keyTypeDataGridViewTextBoxColumn.DataPropertyName = "KeyType";
            this.keyTypeDataGridViewTextBoxColumn.HeaderText = "KeyType";
            this.keyTypeDataGridViewTextBoxColumn.Name = "keyTypeDataGridViewTextBoxColumn";
            this.keyTypeDataGridViewTextBoxColumn.Width = 74;
            // 
            // gTXNameDataGridViewTextBoxColumn
            // 
            this.gTXNameDataGridViewTextBoxColumn.DataPropertyName = "GTXName";
            this.gTXNameDataGridViewTextBoxColumn.HeaderText = "GTXName";
            this.gTXNameDataGridViewTextBoxColumn.Name = "gTXNameDataGridViewTextBoxColumn";
            this.gTXNameDataGridViewTextBoxColumn.Width = 82;
            // 
            // gTXTranspDataGridViewTextBoxColumn
            // 
            this.gTXTranspDataGridViewTextBoxColumn.DataPropertyName = "GTXTransp";
            this.gTXTranspDataGridViewTextBoxColumn.HeaderText = "GTXTransp";
            this.gTXTranspDataGridViewTextBoxColumn.Name = "gTXTranspDataGridViewTextBoxColumn";
            this.gTXTranspDataGridViewTextBoxColumn.Width = 87;
            // 
            // gTXProductDataGridViewTextBoxColumn
            // 
            this.gTXProductDataGridViewTextBoxColumn.DataPropertyName = "GTXProduct";
            this.gTXProductDataGridViewTextBoxColumn.HeaderText = "GTXProduct";
            this.gTXProductDataGridViewTextBoxColumn.Name = "gTXProductDataGridViewTextBoxColumn";
            this.gTXProductDataGridViewTextBoxColumn.Width = 91;
            // 
            // translationRecordBindingSource
            // 
            this.translationRecordBindingSource.DataSource = typeof(UploadDHL.TranslationRecord);
            // 
            // XuFedex
            // 
            this.XuFedex.Location = new System.Drawing.Point(22, 215);
            this.XuFedex.Name = "XuFedex";
            this.XuFedex.Size = new System.Drawing.Size(165, 23);
            this.XuFedex.TabIndex = 10;
            this.XuFedex.Text = "Fedex";
            this.XuFedex.UseVisualStyleBackColor = true;
            this.XuFedex.Click += new System.EventHandler(this.XuFedex_Click);
            // 
            // XuEditTransFedex
            // 
            this.XuEditTransFedex.Location = new System.Drawing.Point(217, 215);
            this.XuEditTransFedex.Name = "XuEditTransFedex";
            this.XuEditTransFedex.Size = new System.Drawing.Size(150, 23);
            this.XuEditTransFedex.TabIndex = 11;
            this.XuEditTransFedex.Text = "Edit Translation";
            this.XuEditTransFedex.UseVisualStyleBackColor = true;
            this.XuEditTransFedex.Click += new System.EventHandler(this.XuEditTransFedex_Click);
            // 
            // XuEditTransGLS
            // 
            this.XuEditTransGLS.Location = new System.Drawing.Point(217, 280);
            this.XuEditTransGLS.Name = "XuEditTransGLS";
            this.XuEditTransGLS.Size = new System.Drawing.Size(150, 23);
            this.XuEditTransGLS.TabIndex = 13;
            this.XuEditTransGLS.Text = "Edit Translation";
            this.XuEditTransGLS.UseVisualStyleBackColor = true;
            // 
            // XuGls
            // 
            this.XuGls.Location = new System.Drawing.Point(22, 280);
            this.XuGls.Name = "XuGls";
            this.XuGls.Size = new System.Drawing.Size(165, 23);
            this.XuGls.TabIndex = 14;
            this.XuGls.Text = "GLS";
            this.XuGls.UseVisualStyleBackColor = true;
            this.XuGls.Click += new System.EventHandler(this.XuGls_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1114, 613);
            this.Controls.Add(this.XuGls);
            this.Controls.Add(this.XuEditTransGLS);
            this.Controls.Add(this.XuEditTransFedex);
            this.Controls.Add(this.XuFedex);
            this.Controls.Add(this.XuEditTranslationGrid);
            this.Controls.Add(this.XuSaveTranslatioon);
            this.Controls.Add(this.XuEditTranslation);
            this.Controls.Add(this.XuMsgGrid);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.FileDone);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.Xu_EditDHL);
            this.Controls.Add(this.XuDHL);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.XuMsgGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuEditTranslationGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.translationRecordBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button XuRun;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button XuDHL;
        private System.Windows.Forms.Button Xu_EditDHL;
        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.Label FileDone;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView XuMsgGrid;
        private System.Windows.Forms.BindingSource gridDataBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filenameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jumpLinesDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button XuEditTranslation;
        private System.Windows.Forms.Button XuSaveTranslatioon;
        private System.Windows.Forms.DataGridView XuEditTranslationGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXTranspDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXProductDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource translationRecordBindingSource;
        private System.Windows.Forms.Button XuFedex;
        private System.Windows.Forms.Button XuEditTransFedex;
        private System.Windows.Forms.Button XuEditTransGLS;
        private System.Windows.Forms.Button XuGls;
    }
}

