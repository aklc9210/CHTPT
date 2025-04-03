namespace Database1ServerApp
{
    partial class DatabaseServerForm
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
            this.lblServerInfo = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrentClient = new DevExpress.XtraEditors.LabelControl();
            this.lstLogs = new DevExpress.XtraEditors.ListBoxControl();
            ((System.ComponentModel.ISupportInitialize)(this.lstLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.Appearance.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblServerInfo.Appearance.Options.UseFont = true;
            this.lblServerInfo.Location = new System.Drawing.Point(472, 27);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(278, 31);
            this.lblServerInfo.TabIndex = 0;
            this.lblServerInfo.Text = "Database Server 1 (Port Y)";
            // 
            // lblCurrentClient
            // 
            this.lblCurrentClient.Location = new System.Drawing.Point(549, 87);
            this.lblCurrentClient.Name = "lblCurrentClient";
            this.lblCurrentClient.Size = new System.Drawing.Size(86, 16);
            this.lblCurrentClient.TabIndex = 1;
            this.lblCurrentClient.Text = "Không có client";
            // 
            // lstLogs
            // 
            this.lstLogs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstLogs.HorizontalScrollbar = true;
            this.lstLogs.Location = new System.Drawing.Point(0, 161);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(1223, 440);
            this.lstLogs.TabIndex = 2;
            // 
            // DatabaseServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 601);
            this.Controls.Add(this.lstLogs);
            this.Controls.Add(this.lblCurrentClient);
            this.Controls.Add(this.lblServerInfo);
            this.Name = "DatabaseServerForm";
            this.Text = "DatabaseServerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatabaseServerForm_FormClosing);
            this.Load += new System.EventHandler(this.DatabaseServerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lstLogs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblServerInfo;
        private DevExpress.XtraEditors.LabelControl lblCurrentClient;
        private DevExpress.XtraEditors.ListBoxControl lstLogs;
    }
}