using DevExpress.XtraEditors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataAccessClientWinForms
{
    public partial class ClientForm : DevExpress.XtraEditors.XtraForm
    {
        private string clientId = "";
        private string coordinatorUrl = "http://192.168.230.103:5000";
        public ClientForm()
        {
            InitializeComponent();

            // Khời tạo id của client từ ban đầu
            clientId = Guid.NewGuid().ToString().Substring(0, 8);
            txtClientId.Text = clientId;
        }


        private async void btnReleaseAccess_Click(object sender, EventArgs e)
        {
            await ReleaseAccessAsync();
        }

        private async void btnRequestAccess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientId.Text))
            {
                MessageBox.Show("Vui lòng nhập Client ID hoặc tạo mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clientId = txtClientId.Text.Trim();
            coordinatorUrl = txtCoordinatorUrl.Text.Trim();
            var jsonRequest = $"{{\"client_id\":\"{clientId}\"}}";
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(coordinatorUrl + "/request_access", content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject resp = JObject.Parse(jsonString);
                        lblAssignedServer.Text = $"Được phân bổ: {resp["server_name"]}";
                        string serverUrl = resp["server_url"].ToString();
                        httpClient.DefaultRequestHeaders.Remove("X-Client-ID");
                        httpClient.DefaultRequestHeaders.Add("X-Client-ID", clientId);
                        var dataResp = await httpClient.GetAsync(serverUrl + "/data");
                        if (dataResp.IsSuccessStatusCode)
                        {
                            string dataJson = await dataResp.Content.ReadAsStringAsync();
                            txtDataDisplay.Text = dataJson;
                        }
                        else
                        {
                            MessageBox.Show("Lỗi khi truy xuất dữ liệu từ Database Server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject resp = JObject.Parse(jsonString);
                        lblAssignedServer.Text = $"Đã kết nối với {resp["server_name"]}";
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();
                        JObject resp = JObject.Parse(jsonString);
                        MessageBox.Show($"Lỗi: {resp["error"]}. {resp["message"]}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Các server đang bận hoặc không thể kết nối." + response.StatusCode + "\n" + responseContent,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerateId_Click(object sender, EventArgs e)
        {
            //clientId = Guid.NewGuid().ToString().Substring(0, 8);
            //txtClientId.Text = clientId;
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            txtCoordinatorUrl.Text = coordinatorUrl;
        }

        private async void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                /*try
                {
                    ReleaseAccessAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi giải phóng quyền truy cập: " + ex.Message);
                }*/
                await ReleaseAccessAsync();
            }
        }

        private async Task ReleaseAccessAsync()
        {
            // Nếu chưa có server được gán thì không cần release
            if (lblAssignedServer.Text == "Chưa có server" || string.IsNullOrWhiteSpace(clientId))
                return;

            var jsonRequest = $"{{\"client_id\":\"{clientId}\"}}";
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(coordinatorUrl + "/release_access", content);
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đã giải phóng quyền truy cập.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblAssignedServer.Text = "Chưa có server";
                        txtDataDisplay.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi khi giải phóng quyền truy cập: " + response.StatusCode + "\n" + responseContent,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}