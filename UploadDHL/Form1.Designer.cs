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
            this.XuMsgGrid = new System.Windows.Forms.DataGridView();
            this.XuSaveTranslatioon = new System.Windows.Forms.Button();
            this.XuEditTranslationGrid = new System.Windows.Forms.DataGridView();
            this.XuEditTransGLS = new System.Windows.Forms.Button();
            this.XuGls = new System.Windows.Forms.Button();
            this.XUHS = new System.Windows.Forms.Button();
            this.XuClose = new System.Windows.Forms.Button();
            this.XU_ArkName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.XuFilterKey = new System.Windows.Forms.TextBox();
            this.XuLabelFilter = new System.Windows.Forms.Label();
            this.XuDataGridError = new System.Windows.Forms.DataGridView();
            this.XuMinDate = new System.Windows.Forms.TextBox();
            this.XuMaxDate = new System.Windows.Forms.TextBox();
            this.XuInvoiceNumber = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.XuNoInvoice = new System.Windows.Forms.CheckBox();
            this.XuHSTranslation = new System.Windows.Forms.Button();
            this.XuTranslationErrorDHL = new System.Windows.Forms.Label();
            this.XuTranslationErrorHS = new System.Windows.Forms.Label();
            this.XuTranslationErrorGLS = new System.Windows.Forms.Label();
            this.XuGetFile = new System.Windows.Forms.Button();
            this.XuErrorMessage = new System.Windows.Forms.Label();
            this.XuShipX = new System.Windows.Forms.Button();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filenameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jumpLinesDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.keyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.keyTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXTranspDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gTXProductDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.translationRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.XuMsgGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuEditTranslationGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuDataGridError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBindingSource)).BeginInit();
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
            this.Xu_EditDHL.Location = new System.Drawing.Point(268, 86);
            this.Xu_EditDHL.Name = "Xu_EditDHL";
            this.Xu_EditDHL.Size = new System.Drawing.Size(158, 23);
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
            this.XuMsgGrid.Location = new System.Drawing.Point(432, 80);
            this.XuMsgGrid.Name = "XuMsgGrid";
            this.XuMsgGrid.Size = new System.Drawing.Size(658, 765);
            this.XuMsgGrid.TabIndex = 6;
            this.XuMsgGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.XuMsgGrid_CellContentClick);
            // 
            // XuSaveTranslatioon
            // 
            this.XuSaveTranslatioon.Location = new System.Drawing.Point(421, 858);
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
            this.XuEditTranslationGrid.Location = new System.Drawing.Point(432, 80);
            this.XuEditTranslationGrid.Name = "XuEditTranslationGrid";
            this.XuEditTranslationGrid.Size = new System.Drawing.Size(658, 772);
            this.XuEditTranslationGrid.TabIndex = 9;
            // 
            // XuEditTransGLS
            // 
            this.XuEditTransGLS.Location = new System.Drawing.Point(268, 154);
            this.XuEditTransGLS.Name = "XuEditTransGLS";
            this.XuEditTransGLS.Size = new System.Drawing.Size(158, 23);
            this.XuEditTransGLS.TabIndex = 13;
            this.XuEditTransGLS.Text = "Edit Translation";
            this.XuEditTransGLS.UseVisualStyleBackColor = true;
            this.XuEditTransGLS.Click += new System.EventHandler(this.XuEditTransGLS_Click);
            // 
            // XuGls
            // 
            this.XuGls.Location = new System.Drawing.Point(22, 154);
            this.XuGls.Name = "XuGls";
            this.XuGls.Size = new System.Drawing.Size(165, 23);
            this.XuGls.TabIndex = 14;
            this.XuGls.Text = "GLS";
            this.XuGls.UseVisualStyleBackColor = true;
            this.XuGls.Click += new System.EventHandler(this.XuGls_Click);
            // 
            // XUHS
            // 
            this.XUHS.Location = new System.Drawing.Point(22, 343);
            this.XUHS.Name = "XUHS";
            this.XUHS.Size = new System.Drawing.Size(165, 23);
            this.XUHS.TabIndex = 16;
            this.XUHS.Text = "HS";
            this.XUHS.UseVisualStyleBackColor = true;
            this.XUHS.Click += new System.EventHandler(this.XUGTX_Click);
            // 
            // XuClose
            // 
            this.XuClose.Location = new System.Drawing.Point(893, 858);
            this.XuClose.Name = "XuClose";
            this.XuClose.Size = new System.Drawing.Size(197, 32);
            this.XuClose.TabIndex = 18;
            this.XuClose.Text = "Close";
            this.XuClose.UseVisualStyleBackColor = true;
            this.XuClose.Click += new System.EventHandler(this.XuClose_Click);
            // 
            // XU_ArkName
            // 
            this.XU_ArkName.Location = new System.Drawing.Point(22, 829);
            this.XU_ArkName.Name = "XU_ArkName";
            this.XU_ArkName.Size = new System.Drawing.Size(171, 20);
            this.XU_ArkName.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(214, 832);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Ark Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // XuFilterKey
            // 
            this.XuFilterKey.Location = new System.Drawing.Point(467, 54);
            this.XuFilterKey.Name = "XuFilterKey";
            this.XuFilterKey.Size = new System.Drawing.Size(92, 20);
            this.XuFilterKey.TabIndex = 25;
            this.XuFilterKey.TextChanged += new System.EventHandler(this.XuFilterChange);
            // 
            // XuLabelFilter
            // 
            this.XuLabelFilter.AutoSize = true;
            this.XuLabelFilter.Location = new System.Drawing.Point(429, 57);
            this.XuLabelFilter.Name = "XuLabelFilter";
            this.XuLabelFilter.Size = new System.Drawing.Size(29, 13);
            this.XuLabelFilter.TabIndex = 26;
            this.XuLabelFilter.Text = "Filter";
            // 
            // XuDataGridError
            // 
            this.XuDataGridError.AllowUserToAddRows = false;
            this.XuDataGridError.AllowUserToDeleteRows = false;
            this.XuDataGridError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.XuDataGridError.Dock = System.Windows.Forms.DockStyle.Top;
            this.XuDataGridError.Location = new System.Drawing.Point(0, 0);
            this.XuDataGridError.Name = "XuDataGridError";
            this.XuDataGridError.ReadOnly = true;
            this.XuDataGridError.Size = new System.Drawing.Size(1114, 324);
            this.XuDataGridError.TabIndex = 28;
            // 
            // XuMinDate
            // 
            this.XuMinDate.Location = new System.Drawing.Point(158, 262);
            this.XuMinDate.Name = "XuMinDate";
            this.XuMinDate.Size = new System.Drawing.Size(100, 20);
            this.XuMinDate.TabIndex = 29;
            // 
            // XuMaxDate
            // 
            this.XuMaxDate.Location = new System.Drawing.Point(264, 262);
            this.XuMaxDate.Name = "XuMaxDate";
            this.XuMaxDate.Size = new System.Drawing.Size(100, 20);
            this.XuMaxDate.TabIndex = 30;
            // 
            // XuInvoiceNumber
            // 
            this.XuInvoiceNumber.Location = new System.Drawing.Point(158, 289);
            this.XuInvoiceNumber.Name = "XuInvoiceNumber";
            this.XuInvoiceNumber.Size = new System.Drawing.Size(206, 20);
            this.XuInvoiceNumber.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 292);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Invoice number";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(85, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 33;
            this.label3.Text = "Date from/to";
            // 
            // XuNoInvoice
            // 
            this.XuNoInvoice.AutoSize = true;
            this.XuNoInvoice.Location = new System.Drawing.Point(22, 63);
            this.XuNoInvoice.Name = "XuNoInvoice";
            this.XuNoInvoice.Size = new System.Drawing.Size(77, 17);
            this.XuNoInvoice.TabIndex = 34;
            this.XuNoInvoice.Text = "No invoice";
            this.XuNoInvoice.UseVisualStyleBackColor = true;
            // 
            // XuHSTranslation
            // 
            this.XuHSTranslation.Location = new System.Drawing.Point(264, 343);
            this.XuHSTranslation.Name = "XuHSTranslation";
            this.XuHSTranslation.Size = new System.Drawing.Size(158, 23);
            this.XuHSTranslation.TabIndex = 35;
            this.XuHSTranslation.Text = "Edit Translation";
            this.XuHSTranslation.UseVisualStyleBackColor = true;
            this.XuHSTranslation.Click += new System.EventHandler(this.XuHSTranslation_Click);
            // 
            // XuTranslationErrorDHL
            // 
            this.XuTranslationErrorDHL.AutoSize = true;
            this.XuTranslationErrorDHL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.XuTranslationErrorDHL.Location = new System.Drawing.Point(265, 70);
            this.XuTranslationErrorDHL.Name = "XuTranslationErrorDHL";
            this.XuTranslationErrorDHL.Size = new System.Drawing.Size(84, 13);
            this.XuTranslationErrorDHL.TabIndex = 36;
            this.XuTranslationErrorDHL.Text = "Translation Error";
            this.XuTranslationErrorDHL.Visible = false;
            this.XuTranslationErrorDHL.Click += new System.EventHandler(this.label4_Click);
            // 
            // XuTranslationErrorHS
            // 
            this.XuTranslationErrorHS.AutoSize = true;
            this.XuTranslationErrorHS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.XuTranslationErrorHS.Location = new System.Drawing.Point(265, 327);
            this.XuTranslationErrorHS.Name = "XuTranslationErrorHS";
            this.XuTranslationErrorHS.Size = new System.Drawing.Size(84, 13);
            this.XuTranslationErrorHS.TabIndex = 37;
            this.XuTranslationErrorHS.Text = "Translation Error";
            this.XuTranslationErrorHS.Visible = false;
            // 
            // XuTranslationErrorGLS
            // 
            this.XuTranslationErrorGLS.AutoSize = true;
            this.XuTranslationErrorGLS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.XuTranslationErrorGLS.Location = new System.Drawing.Point(265, 138);
            this.XuTranslationErrorGLS.Name = "XuTranslationErrorGLS";
            this.XuTranslationErrorGLS.Size = new System.Drawing.Size(84, 13);
            this.XuTranslationErrorGLS.TabIndex = 38;
            this.XuTranslationErrorGLS.Text = "Translation Error";
            this.XuTranslationErrorGLS.Visible = false;
            // 
            // XuGetFile
            // 
            this.XuGetFile.Location = new System.Drawing.Point(22, 858);
            this.XuGetFile.Name = "XuGetFile";
            this.XuGetFile.Size = new System.Drawing.Size(197, 32);
            this.XuGetFile.TabIndex = 39;
            this.XuGetFile.Text = "Upload file";
            this.XuGetFile.UseVisualStyleBackColor = true;
            this.XuGetFile.Click += new System.EventHandler(this.XuGetFile_Click);
            // 
            // XuErrorMessage
            // 
            this.XuErrorMessage.AutoSize = true;
            this.XuErrorMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.XuErrorMessage.Location = new System.Drawing.Point(19, 379);
            this.XuErrorMessage.Name = "XuErrorMessage";
            this.XuErrorMessage.Size = new System.Drawing.Size(0, 13);
            this.XuErrorMessage.TabIndex = 40;
            // 
            // XuShipX
            // 
            this.XuShipX.Location = new System.Drawing.Point(22, 446);
            this.XuShipX.Name = "XuShipX";
            this.XuShipX.Size = new System.Drawing.Size(165, 23);
            this.XuShipX.TabIndex = 41;
            this.XuShipX.Text = "ShipX";
            this.XuShipX.UseVisualStyleBackColor = true;
            this.XuShipX.Click += new System.EventHandler(this.XuShipX_Click);
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
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1114, 930);
            this.Controls.Add(this.XuDataGridError);
            this.Controls.Add(this.XuShipX);
            this.Controls.Add(this.XuErrorMessage);
            this.Controls.Add(this.XuGetFile);
            this.Controls.Add(this.XuTranslationErrorGLS);
            this.Controls.Add(this.XuTranslationErrorHS);
            this.Controls.Add(this.XuTranslationErrorDHL);
            this.Controls.Add(this.XuHSTranslation);
            this.Controls.Add(this.XuNoInvoice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.XuInvoiceNumber);
            this.Controls.Add(this.XuMaxDate);
            this.Controls.Add(this.XuMinDate);
            this.Controls.Add(this.XuClose);
            this.Controls.Add(this.XuLabelFilter);
            this.Controls.Add(this.XuFilterKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.XU_ArkName);
            this.Controls.Add(this.XUHS);
            this.Controls.Add(this.XuGls);
            this.Controls.Add(this.XuEditTransGLS);
            this.Controls.Add(this.XuSaveTranslatioon);
            this.Controls.Add(this.XuMsgGrid);
            this.Controls.Add(this.FileDone);
            this.Controls.Add(this.Message);
            this.Controls.Add(this.Xu_EditDHL);
            this.Controls.Add(this.XuDHL);
            this.Controls.Add(this.XuEditTranslationGrid);
            this.Name = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.XuMsgGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuEditTranslationGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.XuDataGridError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDataBindingSource)).EndInit();
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
        private System.Windows.Forms.DataGridView XuMsgGrid;
        private System.Windows.Forms.BindingSource gridDataBindingSource;
        private System.Windows.Forms.Button XuSaveTranslatioon;
        private System.Windows.Forms.DataGridView XuEditTranslationGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn keyTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXTranspDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn gTXProductDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource translationRecordBindingSource;
        private System.Windows.Forms.Button XuEditTransGLS;
        private System.Windows.Forms.Button XuGls;
        private System.Windows.Forms.Button XUHS;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filenameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jumpLinesDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button XuClose;
        private System.Windows.Forms.TextBox XU_ArkName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox XuFilterKey;
        private System.Windows.Forms.Label XuLabelFilter;
        private System.Windows.Forms.DataGridView XuDataGridError;
        private System.Windows.Forms.TextBox XuMinDate;
        private System.Windows.Forms.TextBox XuMaxDate;
        private System.Windows.Forms.TextBox XuInvoiceNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox XuNoInvoice;
        private System.Windows.Forms.Button XuHSTranslation;
        private System.Windows.Forms.Label XuTranslationErrorDHL;
        private System.Windows.Forms.Label XuTranslationErrorHS;
        private System.Windows.Forms.Label XuTranslationErrorGLS;
        private System.Windows.Forms.Button XuGetFile;
        private System.Windows.Forms.Label XuErrorMessage;
        private System.Windows.Forms.Button XuShipX;
    }
}

