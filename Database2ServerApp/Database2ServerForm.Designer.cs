namespace Database2ServerApp
{
    partial class Database2ServerForm
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.lstLogs = new DevExpress.XtraEditors.ListBoxControl();
            this.lblServerInfo = new DevExpress.XtraEditors.LabelControl();
            this.lblCurrentClient = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstLogs)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblCurrentClient);
            this.panelControl1.Controls.Add(this.lblServerInfo);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1182, 116);
            this.panelControl1.TabIndex = 0;
            // 
            // lstLogs
            // 
            this.lstLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLogs.Location = new System.Drawing.Point(0, 116);
            this.lstLogs.Name = "lstLogs";
            this.lstLogs.Size = new System.Drawing.Size(1182, 492);
            this.lstLogs.TabIndex = 1;
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.Appearance.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerInfo.Appearance.Options.UseFont = true;
            this.lblServerInfo.Location = new System.Drawing.Point(452, 21);
            this.lblServerInfo.Name = "lblServerInfo";
            this.lblServerInfo.Size = new System.Drawing.Size(278, 31);
            this.lblServerInfo.TabIndex = 0;
            this.lblServerInfo.Text = "Database Server 2 (Port Y)";
            // 
            // lblCurrentClient
            // 
            this.lblCurrentClient.Location = new System.Drawing.Point(548, 73);
            this.lblCurrentClient.Name = "lblCurrentClient";
            this.lblCurrentClient.Size = new System.Drawing.Size(86, 16);
            this.lblCurrentClient.TabIndex = 1;
            this.lblCurrentClient.Text = "Không có client";
            // 
            // Database2ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 608);
            this.Controls.Add(this.lstLogs);
            this.Controls.Add(this.panelControl1);
            this.Name = "Database2ServerForm";
            this.Text = "Database Server 2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Database2ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.Database2ServerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstLogs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl lblCurrentClient;
        private DevExpress.XtraEditors.LabelControl lblServerInfo;
        private DevExpress.XtraEditors.ListBoxControl lstLogs;
    }
}