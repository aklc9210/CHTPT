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
/*        private string coordinatorUrl = "http://192.168.214.103:5000";*/
        private string coordinatorUrl = "http://localhost:5000";

        private System.Windows.Forms.Timer timerCheckLock;

        public ClientForm()
        {
            InitializeComponent();
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

                        if (timerCheckLock == null)
                        {
                            timerCheckLock = new System.Windows.Forms.Timer();
                            timerCheckLock.Interval = 1000; // 5 giây một lần
                            timerCheckLock.Tick += TimerCheckLock_Tick;
                        }
                        timerCheckLock.Start();

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
            clientId = Guid.NewGuid().ToString().Substring(0, 8);
            txtClientId.Text = clientId;
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

                        // Sau khi release xong, dừng timerCheckLock
                        timerCheckLock?.Stop();

                        // Cập nhật UI
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

        private async void TimerCheckLock_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientId) || lblAssignedServer.Text == "Chưa có server")
                return;

            try
            {
                using (var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(2) })
                {
                    // Tạo url kiểm tra trạng thái
                    string url = $"{coordinatorUrl}/check_lock_status?client_id={Uri.EscapeDataString(clientId)}";
                    var response = await httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                        return;

                    string json = await response.Content.ReadAsStringAsync();
                    var obj = JObject.Parse(json);
                    bool holding = obj["holding"].Value<bool>();
                    bool forced = obj["forced_released"].Value<bool>();


                    if (!holding && lblAssignedServer.Text.StartsWith("Được phân bổ"))
                    {
                        // Dừng timer
                        timerCheckLock.Stop();

                        // Nếu forced == true → DataServer sập đang buộc thu hồi lock
                        if (forced)
                        {
                            MessageBox.Show(
                                "Quyền truy cập của bạn đã bị thu hồi (DataServer sập).",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Nếu forced = false nhưng holding = false: client đã release bình thường hoặc không được cấp lock
                            MessageBox.Show(
                                "Bạn đã mất quyền truy cập.",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }

                        // Cập nhật UI của client về trạng thái “Chưa có server”
                        lblAssignedServer.Text = "Chưa có server";
                        txtDataDisplay.Clear();
                    }
                }
            }
            catch
            {
                // Nếu không thể kết nối tới Coordinator, lần sau vẫn thử lại
            }
        }


    }
}