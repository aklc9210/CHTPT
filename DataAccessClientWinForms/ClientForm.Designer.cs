namespace DataAccessClientWinForms
{
    partial class ClientForm
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
            this.lblClientId = new DevExpress.XtraEditors.LabelControl();
            this.txtClientId = new DevExpress.XtraEditors.TextEdit();
            this.btnGenerateId = new System.Windows.Forms.Button();
            this.lblCoordinatorUrl = new DevExpress.XtraEditors.LabelControl();
            this.txtCoordinatorUrl = new DevExpress.XtraEditors.TextEdit();
            this.btnRequestAccess = new System.Windows.Forms.Button();
            this.btnReleaseAccess = new System.Windows.Forms.Button();
            this.lblAssignedServer = new DevExpress.XtraEditors.LabelControl();
            this.txtDataDisplay = new System.Windows.Forms.TextBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCoordinatorUrl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblClientId
            // 
            this.lblClientId.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblClientId.Appearance.Options.UseFont = true;
            this.lblClientId.Location = new System.Drawing.Point(34, 46);
            this.lblClientId.Name = "lblClientId";
            this.lblClientId.Size = new System.Drawing.Size(74, 28);
            this.lblClientId.TabIndex = 0;
            this.lblClientId.Text = "Client ID";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(217, 43);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtClientId.Properties.Appearance.Options.UseFont = true;
            this.txtClientId.Size = new System.Drawing.Size(237, 34);
            this.txtClientId.TabIndex = 1;
            // 
            // btnGenerateId
            // 
            this.btnGenerateId.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnGenerateId.Location = new System.Drawing.Point(484, 37);
            this.btnGenerateId.Name = "btnGenerateId";
            this.btnGenerateId.Size = new System.Drawing.Size(138, 47);
            this.btnGenerateId.TabIndex = 2;
            this.btnGenerateId.Text = "Tạo ID";
            this.btnGenerateId.UseVisualStyleBackColor = true;
            this.btnGenerateId.Click += new System.EventHandler(this.btnGenerateId_Click);
            // 
            // lblCoordinatorUrl
            // 
            this.lblCoordinatorUrl.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblCoordinatorUrl.Appearance.Options.UseFont = true;
            this.lblCoordinatorUrl.Location = new System.Drawing.Point(34, 121);
            this.lblCoordinatorUrl.Name = "lblCoordinatorUrl";
            this.lblCoordinatorUrl.Size = new System.Drawing.Size(147, 28);
            this.lblCoordinatorUrl.TabIndex = 3;
            this.lblCoordinatorUrl.Text = "Coordinator URL";
            // 
            // txtCoordinatorUrl
            // 
            this.txtCoordinatorUrl.Location = new System.Drawing.Point(217, 118);
            this.txtCoordinatorUrl.Name = "txtCoordinatorUrl";
            this.txtCoordinatorUrl.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtCoordinatorUrl.Properties.Appearance.Options.UseFont = true;
            this.txtCoordinatorUrl.Size = new System.Drawing.Size(405, 34);
            this.txtCoordinatorUrl.TabIndex = 4;
            // 
            // btnRequestAccess
            // 
            this.btnRequestAccess.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnRequestAccess.Location = new System.Drawing.Point(205, 200);
            this.btnRequestAccess.Name = "btnRequestAccess";
            this.btnRequestAccess.Size = new System.Drawing.Size(191, 40);
            this.btnRequestAccess.TabIndex = 5;
            this.btnRequestAccess.Text = "Yêu cầu truy cập";
            this.btnRequestAccess.UseVisualStyleBackColor = true;
            this.btnRequestAccess.Click += new System.EventHandler(this.btnRequestAccess_Click);
            // 
            // btnReleaseAccess
            // 
            this.btnReleaseAccess.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnReleaseAccess.Location = new System.Drawing.Point(450, 200);
            this.btnReleaseAccess.Name = "btnReleaseAccess";
            this.btnReleaseAccess.Size = new System.Drawing.Size(172, 40);
            this.btnReleaseAccess.TabIndex = 6;
            this.btnReleaseAccess.Text = "Giải phóng quyền";
            this.btnReleaseAccess.UseVisualStyleBackColor = true;
            this.btnReleaseAccess.Click += new System.EventHandler(this.btnReleaseAccess_Click);
            // 
            // lblAssignedServer
            // 
            this.lblAssignedServer.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblAssignedServer.Appearance.Options.UseFont = true;
            this.lblAssignedServer.Location = new System.Drawing.Point(215, 37);
            this.lblAssignedServer.Name = "lblAssignedServer";
            this.lblAssignedServer.Size = new System.Drawing.Size(128, 28);
            this.lblAssignedServer.TabIndex = 7;
            this.lblAssignedServer.Text = "Chưa có server";
            // 
            // txtDataDisplay
            // 
            this.txtDataDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtDataDisplay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.txtDataDisplay.Location = new System.Drawing.Point(2, 98);
            this.txtDataDisplay.Multiline = true;
            this.txtDataDisplay.Name = "txtDataDisplay";
            this.txtDataDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDataDisplay.Size = new System.Drawing.Size(550, 464);
            this.txtDataDisplay.TabIndex = 8;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.lblClientId);
            this.panelControl1.Controls.Add(this.txtCoordinatorUrl);
            this.panelControl1.Controls.Add(this.lblCoordinatorUrl);
            this.panelControl1.Controls.Add(this.btnReleaseAccess);
            this.panelControl1.Controls.Add(this.txtClientId);
            this.panelControl1.Controls.Add(this.btnRequestAccess);
            this.panelControl1.Controls.Add(this.btnGenerateId);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(658, 564);
            this.panelControl1.TabIndex = 9;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtDataDisplay);
            this.panelControl2.Controls.Add(this.lblAssignedServer);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(658, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(554, 564);
            this.panelControl2.TabIndex = 10;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 564);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtClientId.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCoordinatorUrl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblClientId;
        private DevExpress.XtraEditors.TextEdit txtClientId;
        private System.Windows.Forms.Button btnGenerateId;
        private DevExpress.XtraEditors.LabelControl lblCoordinatorUrl;
        private DevExpress.XtraEditors.TextEdit txtCoordinatorUrl;
        private System.Windows.Forms.Button btnRequestAccess;
        private System.Windows.Forms.Button btnReleaseAccess;
        private DevExpress.XtraEditors.LabelControl lblAssignedServer;
        private System.Windows.Forms.TextBox txtDataDisplay;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}