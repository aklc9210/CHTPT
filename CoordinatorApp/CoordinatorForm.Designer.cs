namespace CoordinatorApp
{
    partial class CoordinatorForm
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
            this.dataGridViewServers = new System.Windows.Forms.DataGridView();
            this.colServerId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastAccess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.lblTitle = new DevExpress.XtraEditors.LabelControl();
            this.pnlHeader = new DevExpress.XtraEditors.PanelControl();
            this.lblSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.lstNotifications = new DevExpress.XtraEditors.ListBoxControl();
            this.grpConnection = new System.Windows.Forms.GroupBox();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.txtCoordinatorUrl = new DevExpress.XtraEditors.TextEdit();
            this.lblCoordinatorUrl = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstNotifications)).BeginInit();
            this.grpConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCoordinatorUrl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewServers
            // 
            this.dataGridViewServers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewServers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewServers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colServerId,
            this.colName,
            this.colUrl,
            this.colStatus,
            this.colClient,
            this.colLastAccess});
            this.dataGridViewServers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridViewServers.Location = new System.Drawing.Point(0, 406);
            this.dataGridViewServers.Name = "dataGridViewServers";
            this.dataGridViewServers.RowHeadersWidth = 51;
            this.dataGridViewServers.RowTemplate.Height = 24;
            this.dataGridViewServers.Size = new System.Drawing.Size(1188, 245);
            this.dataGridViewServers.TabIndex = 0;
            // 
            // colServerId
            // 
            this.colServerId.HeaderText = "Server ID";
            this.colServerId.MinimumWidth = 6;
            this.colServerId.Name = "colServerId";
            this.colServerId.ReadOnly = true;
            // 
            // colName
            // 
            this.colName.HeaderText = "Tên ";
            this.colName.MinimumWidth = 6;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colUrl
            // 
            this.colUrl.HeaderText = "URL ";
            this.colUrl.MinimumWidth = 6;
            this.colUrl.Name = "colUrl";
            this.colUrl.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colClient
            // 
            this.colClient.HeaderText = "Client ";
            this.colClient.MinimumWidth = 6;
            this.colClient.Name = "colClient";
            this.colClient.ReadOnly = true;
            // 
            // colLastAccess
            // 
            this.colLastAccess.HeaderText = "Last Access";
            this.colLastAccess.MinimumWidth = 6;
            this.colLastAccess.Name = "colLastAccess";
            this.colLastAccess.ReadOnly = true;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.TimerRefresh_Tick);
            // 
            // lblTitle
            // 
            this.lblTitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblTitle.Appearance.Options.UseFont = true;
            this.lblTitle.Location = new System.Drawing.Point(378, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(335, 38);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Hệ thống Điều phối CSDL";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Appearance.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Appearance.Options.UseBackColor = true;
            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1188, 106);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Location = new System.Drawing.Point(353, 64);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(375, 16);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Giám sát và quản lý trạng thái truy cập dữ liệu theo thời gian thực";
            // 
            // lstNotifications
            // 
            this.lstNotifications.Dock = System.Windows.Forms.DockStyle.Right;
            this.lstNotifications.HorizontalScrollbar = true;
            this.lstNotifications.Location = new System.Drawing.Point(600, 106);
            this.lstNotifications.Name = "lstNotifications";
            this.lstNotifications.Size = new System.Drawing.Size(588, 300);
            this.lstNotifications.TabIndex = 3;
            // 
            // grpConnection
            // 
            this.grpConnection.Controls.Add(this.btnReconnect);
            this.grpConnection.Controls.Add(this.txtCoordinatorUrl);
            this.grpConnection.Controls.Add(this.lblCoordinatorUrl);
            this.grpConnection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpConnection.Location = new System.Drawing.Point(0, 106);
            this.grpConnection.Name = "grpConnection";
            this.grpConnection.Size = new System.Drawing.Size(600, 300);
            this.grpConnection.TabIndex = 4;
            this.grpConnection.TabStop = false;
            this.grpConnection.Text = "groupBox1";
            // 
            // btnReconnect
            // 
            this.btnReconnect.Location = new System.Drawing.Point(339, 60);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(104, 23);
            this.btnReconnect.TabIndex = 2;
            this.btnReconnect.Text = "Kết nối lại";
            this.btnReconnect.UseVisualStyleBackColor = true;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // txtCoordinatorUrl
            // 
            this.txtCoordinatorUrl.EditValue = "http://localhost:5000";
            this.txtCoordinatorUrl.Location = new System.Drawing.Point(137, 61);
            this.txtCoordinatorUrl.Name = "txtCoordinatorUrl";
            this.txtCoordinatorUrl.Size = new System.Drawing.Size(172, 22);
            this.txtCoordinatorUrl.TabIndex = 1;
            this.txtCoordinatorUrl.EditValueChanged += new System.EventHandler(this.textEdit1_EditValueChanged);
            // 
            // lblCoordinatorUrl
            // 
            this.lblCoordinatorUrl.Location = new System.Drawing.Point(42, 64);
            this.lblCoordinatorUrl.Name = "lblCoordinatorUrl";
            this.lblCoordinatorUrl.Size = new System.Drawing.Size(75, 16);
            this.lblCoordinatorUrl.TabIndex = 0;
            this.lblCoordinatorUrl.Text = "labelControl1";
            // 
            // CoordinatorForm
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1188, 651);
            this.Controls.Add(this.grpConnection);
            this.Controls.Add(this.lstNotifications);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.dataGridViewServers);
            this.Name = "CoordinatorForm";
            this.Text = "CoordinatorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CoordinatorForm_FormClosing);
            this.Load += new System.EventHandler(this.CoordinatorForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewServers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlHeader)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstNotifications)).EndInit();
            this.grpConnection.ResumeLayout(false);
            this.grpConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCoordinatorUrl.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewServers;
        private System.Windows.Forms.Timer timerRefresh;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.PanelControl pnlHeader;
        private DevExpress.XtraEditors.LabelControl lblSubtitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServerId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastAccess;
        private DevExpress.XtraEditors.ListBoxControl lstNotifications;
        private System.Windows.Forms.GroupBox grpConnection;
        private DevExpress.XtraEditors.TextEdit txtCoordinatorUrl;
        private DevExpress.XtraEditors.LabelControl lblCoordinatorUrl;
        private System.Windows.Forms.Button btnReconnect;
    }
}