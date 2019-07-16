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
            this.XuPdk = new System.Windows.Forms.Button();
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
            this.XUGTX = new System.Windows.Forms.Button();
            this.XuGTXTrans = new System.Windows.Forms.Button();
            this.XuJumpLines = new System.Windows.Forms.TextBox();
            this.XuClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.XuZeroCost = new System.Windows.Forms.Button();
            this.XuSplitWeigth = new System.Windows.Forms.Button();
            this.XuRasFileMaker = new System.Windows.Forms.Button();
            this.XU_ArkName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.XuFilterKey = new System.Windows.Forms.TextBox();
            this.XuLabelFilter = new System.Windows.Forms.Label();
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
            // XuPdk
            // 
            this.XuPdk.Location = new System.Drawing.Point(22, 149);
            this.XuPdk.Name = "XuPdk";
            this.XuPdk.Size = new System.Drawing.Size(165, 23);
            this.XuPdk.TabIndex = 4;
            this.XuPdk.Text = "POSTNORD";
            this.XuPdk.UseVisualStyleBackColor = true;
            this.XuPdk.Click += new System.EventHandler(this.XuPdk_Click);
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
            this.XuMsgGrid.Size = new System.Drawing.Size(658, 485);
            this.XuMsgGrid.TabIndex = 6;
            this.XuMsgGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.XuMsgGrid_CellContentClick);
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
            this.XuEditTransGLS.Click += new System.EventHandler(this.XuEditTransGLS_Click);
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
            // XUGTX
            // 
            this.XUGTX.Location = new System.Drawing.Point(22, 343);
            this.XUGTX.Name = "XUGTX";
            this.XUGTX.Size = new System.Drawing.Size(165, 23);
            this.XUGTX.TabIndex = 16;
            this.XUGTX.Text = "GTX";
            this.XUGTX.UseVisualStyleBackColor = true;
            this.XUGTX.Click += new System.EventHandler(this.XUGTX_Click);
            // 
            // XuGTXTrans
            // 
            this.XuGTXTrans.Location = new System.Drawing.Point(217, 343);
            this.XuGTXTrans.Name = "XuGTXTrans";
            this.XuGTXTrans.Size = new System.Drawing.Size(150, 23);
            this.XuGTXTrans.TabIndex = 15;
            this.XuGTXTrans.Text = "Edit Translation";
            this.XuGTXTrans.UseVisualStyleBackColor = true;
            this.XuGTXTrans.Click += new System.EventHandler(this.XuGTXTrans_Click);
            // 
            // XuJumpLines
            // 
            this.XuJumpLines.Location = new System.Drawing.Point(432, 127);
            this.XuJumpLines.Multiline = true;
            this.XuJumpLines.Name = "XuJumpLines";
            this.XuJumpLines.Size = new System.Drawing.Size(658, 725);
            this.XuJumpLines.TabIndex = 17;
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
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(22, 565);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(290, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "Split Access file";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // XuZeroCost
            // 
            this.XuZeroCost.Location = new System.Drawing.Point(22, 456);
            this.XuZeroCost.Name = "XuZeroCost";
            this.XuZeroCost.Size = new System.Drawing.Size(75, 23);
            this.XuZeroCost.TabIndex = 20;
            this.XuZeroCost.Text = "ZeroCost";
            this.XuZeroCost.UseVisualStyleBackColor = true;
            this.XuZeroCost.Visible = false;
            this.XuZeroCost.Click += new System.EventHandler(this.XuZeroCost_Click);
            // 
            // XuSplitWeigth
            // 
            this.XuSplitWeigth.Location = new System.Drawing.Point(22, 505);
            this.XuSplitWeigth.Name = "XuSplitWeigth";
            this.XuSplitWeigth.Size = new System.Drawing.Size(290, 23);
            this.XuSplitWeigth.TabIndex = 21;
            this.XuSplitWeigth.Text = "SplitWeightFile";
            this.XuSplitWeigth.UseVisualStyleBackColor = true;
            this.XuSplitWeigth.Click += new System.EventHandler(this.XuSplitWeigth_Click);
            // 
            // XuRasFileMaker
            // 
            this.XuRasFileMaker.Location = new System.Drawing.Point(22, 485);
            this.XuRasFileMaker.Name = "XuRasFileMaker";
            this.XuRasFileMaker.Size = new System.Drawing.Size(75, 23);
            this.XuRasFileMaker.TabIndex = 22;
            this.XuRasFileMaker.Text = "RasFileMaker";
            this.XuRasFileMaker.UseVisualStyleBackColor = true;
            this.XuRasFileMaker.Visible = false;
            this.XuRasFileMaker.Click += new System.EventHandler(this.XuRasFileMaker_Click);
            // 
            // XU_ArkName
            // 
            this.XU_ArkName.Location = new System.Drawing.Point(22, 399);
            this.XU_ArkName.Name = "XU_ArkName";
            this.XU_ArkName.Size = new System.Drawing.Size(171, 20);
            this.XU_ArkName.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 405);
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
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1114, 930);
            this.Controls.Add(this.XuLabelFilter);
            this.Controls.Add(this.XuFilterKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.XU_ArkName);
            this.Controls.Add(this.XuRasFileMaker);
            this.Controls.Add(this.XuSplitWeigth);
            this.Controls.Add(this.XuZeroCost);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.XuClose);
            this.Controls.Add(this.XuJumpLines);
            this.Controls.Add(this.XUGTX);
            this.Controls.Add(this.XuGTXTrans);
            this.Controls.Add(this.XuGls);
            this.Controls.Add(this.XuEditTransGLS);
            this.Controls.Add(this.XuEditTransFedex);
            this.Controls.Add(this.XuFedex);
            this.Controls.Add(this.XuEditTranslationGrid);
            this.Controls.Add(this.XuSaveTranslatioon);
            this.Controls.Add(this.XuEditTranslation);
            this.Controls.Add(this.XuMsgGrid);
            this.Controls.Add(this.XuPdk);
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
        private System.Windows.Forms.Button XuPdk;
        private System.Windows.Forms.DataGridView XuMsgGrid;
        private System.Windows.Forms.BindingSource gridDataBindingSource;
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
        private System.Windows.Forms.Button XUGTX;
        private System.Windows.Forms.Button XuGTXTrans;
        private System.Windows.Forms.TextBox XuJumpLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn filenameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jumpLinesDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button XuClose;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button XuZeroCost;
        private System.Windows.Forms.Button XuSplitWeigth;
        private System.Windows.Forms.Button XuRasFileMaker;
        private System.Windows.Forms.TextBox XU_ArkName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox XuFilterKey;
        private System.Windows.Forms.Label XuLabelFilter;
    }
}

