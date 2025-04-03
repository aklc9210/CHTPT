using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoordinatorApp
{
    public partial class CoordinatorForm : DevExpress.XtraEditors.XtraForm
    {
        public CoordinatorForm()
        {
            InitializeComponent();
            InitializeServerState();
        }

        // HttpListener để lắng nghe các request
        private HttpListener httpListener;
        private CancellationTokenSource cts;
        private const string listenUrl = "http://+:5000/";

        // Lock để đồng bộ truy cập trạng thái server
        private readonly object stateLock = new object();

        // Danh sách các database server
        private List<DatabaseServer> servers;

        // Khởi tạo danh sách server mẫu
        private void InitializeServerState()
        {
            servers = new List<DatabaseServer>
            {
                new DatabaseServer
                {
                    Id = 1,
                    Name = "Database Server 1",
                    Url = "http://192.168.214.103:5001",
                    Busy = false,
                    CurrentClient = "",
                    LastAccess = DateTime.MinValue
                },
                new DatabaseServer
                {
                    Id = 2,
                    Name = "Database Server 2",
                    Url = "http://192.168.214.103:5002",
                    Busy = false,
                    CurrentClient = "",
                    LastAccess = DateTime.MinValue
                }
            };
        }

        private void AddNotification(string message, string type)
        {
            // Kiểm tra xem có đang ở UI thread không
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddNotification(message, type)));
                return;
            }
            // Nếu đang ở UI thread, cập nhật giao diện bình thường
            string prefix = $"[{DateTime.Now:T}] ";
            lstNotifications.Items.Insert(0, prefix + message);
        }


        private void CoordinatorForm_Load(object sender, EventArgs e)
        {
            // Khởi động HttpListener trên thread riêng
            cts = new CancellationTokenSource();
            Task.Run(() => StartHttpListener(cts.Token));
            timerRefresh.Start();

            AddNotification("Coordinator khởi động thành công.", "info");

            // Khởi động timer để cập nhật dashboard (DataGridView)
            /*timerFresher.Interval = 5000;
            timerFresher.Tick += TimerRefresh_Tick;
            timerFresher.Start();*/
        }

        private void TimerRefresh_Tick(object sender, EventArgs e)
        {
            UpdateServerGrid();
        }

        // Cập nhật DataGridView (dataGridViewServers) với thông tin từ danh sách server
        private void UpdateServerGrid()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateServerGrid));
                return;
            }
            dataGridViewServers.Rows.Clear();
            lock (stateLock)
            {
                foreach (var server in servers)
                {
                    string state = server.Busy ? "Đang bận" : "Sẵn sàng";
                    string lastAccess = server.LastAccess == DateTime.MinValue ? "" : server.LastAccess.ToString("HH:mm:ss");
                    dataGridViewServers.Rows.Add(server.Id, server.Name, server.Url, state, server.CurrentClient, lastAccess);
                }
            }
        }

        // Hàm lắng nghe các request HTTP
        private async Task StartHttpListener(CancellationToken token)
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(listenUrl);
            try
            {
                httpListener.Start();
               /* AddNotification("HttpListener khởi động tại " + listenUrl, "success");*/
            }
            catch (HttpListenerException ex)
            {
                MessageBox.Show("Không thể khởi động HttpListener: " + ex.Message);
                return;
            }

            while (!token.IsCancellationRequested)
            {
                try
                {
                    var context = await httpListener.GetContextAsync();
                    _ = Task.Run(() => ProcessRequest(context));
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        AddNotification("Lỗi listener: " + ex.Message, "error");
                        Console.WriteLine("Lỗi listener: " + ex.Message);
                    }
                        

                }
            }
        }

        // Xử lý request dựa theo URL và method
        private async void ProcessRequest(HttpListenerContext context)
        {
            string responseString = "";
            int statusCode = 200;
            try
            {
                string rawUrl = context.Request.RawUrl;
                string method = context.Request.HttpMethod;
                if (rawUrl.StartsWith("/request_access") && method == "POST")
                {
                    await HandleRequestAccess(context);
                    return;
                }
                else if (rawUrl.StartsWith("/release_access") && method == "POST")
                {
                    await HandleReleaseAccess(context);
                    return;
                }
                else if (rawUrl.StartsWith("/server_status") && method == "GET")
                {
                    responseString = GetServerStatusJson();
                }
                else
                {
                    statusCode = 404;
                    responseString = "{\"error\":\"Endpoint not found\"}";
                }
            }
            catch (Exception ex)
            {
                statusCode = 500;
                responseString = JsonConvert.SerializeObject(new { error = ex.Message });
            }
            // Gửi response
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            using (var output = context.Response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        // Xử lý POST /request_access
        private async Task HandleRequestAccess(HttpListenerContext context)
        {
            string body;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }
            var req = JsonConvert.DeserializeObject<RequestAccessRequest>(body);
            if (req == null || string.IsNullOrWhiteSpace(req.client_id))
            {
                context.Response.StatusCode = 400;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "Client ID is required" }));
                return;
            }
            DatabaseServer assignedServer = null;
            lock (stateLock)
            {
                // Kiểm tra xem client đã được kết nối chưa
                foreach (var server in servers)
                {
                    if (server.CurrentClient == req.client_id)
                    {
                        assignedServer = server;
                        break;
                    }
                }
                if (assignedServer != null)
                {
                    context.Response.StatusCode = 409;
                    var respConflict = new
                    {
                        error = "Client already connected",
                        message = $"Client {req.client_id} đã đang truy cập {assignedServer.Name}",
                        server_id = assignedServer.Id,
                        server_name = assignedServer.Name,
                        server_url = assignedServer.Url
                    };
                    WriteResponse(context, JsonConvert.SerializeObject(respConflict)).Wait();
                    return;
                }
                // Kiểm tra số lượng server bận
                int busyCount = 0;
                foreach (var server in servers)
                {
                    if (server.Busy)
                        busyCount++;
                }
                if (busyCount >= servers.Count)
                {
                    context.Response.StatusCode = 503;
                    var respBusy = new
                    {
                        error = "All database servers are busy",
                        message = "Tất cả các database server đang bận."
                    };
                    WriteResponse(context, JsonConvert.SerializeObject(respBusy)).Wait();
                    return;
                }
                // Chọn server rảnh, nếu có nhiều, chọn server có LastAccess nhỏ nhất
                List<DatabaseServer> available = servers.FindAll(s => !s.Busy);
                assignedServer = available[0];
                foreach (var s in available)
                {
                    if (s.LastAccess < assignedServer.LastAccess)
                        assignedServer = s;
                }
                // Cập nhật trạng thái server
                assignedServer.Busy = true;
                assignedServer.CurrentClient = req.client_id;
                assignedServer.LastAccess = DateTime.Now;

            }

            using (HttpClient httpClient = new HttpClient())
            {
                var notifyPayload = JsonConvert.SerializeObject(new { client_id = req.client_id });
                var notifyContent = new StringContent(notifyPayload, Encoding.UTF8, "application/json");
                try
                {
                    var notifyResponse = await httpClient.PostAsync(assignedServer.Url + "/notify_access", notifyContent);
                    // Nếu cần, kiểm tra notifyResponse
                    AddNotification($"Đã thông báo cho {assignedServer.Name} về client {req.client_id}.", "info");
                }
                catch (Exception ex)
                {
                    AddNotification("Lỗi thông báo Dataserver: " + ex.Message, "error");
                    // Có thể xử lý reset trạng thái server ở đây nếu cần
                }
            }

            // Trả về thông tin server đã được phân phối
            var respSuccess = new
            {
                server_id = assignedServer.Id,
                server_name = assignedServer.Name,
                server_url = assignedServer.Url
            };
            await WriteResponse(context, JsonConvert.SerializeObject(respSuccess));
        }

        // Xử lý POST /release_access
        private async Task HandleReleaseAccess(HttpListenerContext context)
        {
            string body;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }
            var req = JsonConvert.DeserializeObject<ReleaseAccessRequest>(body);
            if (req == null || string.IsNullOrWhiteSpace(req.client_id))
            {
                context.Response.StatusCode = 400;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "Client ID is required" }));
                return;
            }

            DatabaseServer releasedServer = null;
            lock (stateLock)
            {
                foreach (var server in servers)
                {
                    if (server.CurrentClient == req.client_id)
                    {
                        server.Busy = false;
                        server.CurrentClient = "";
                        server.LastAccess = DateTime.Now;
                        releasedServer = server;
                        break;
                    }
                }
            }

            if (releasedServer != null)
            {
                // Gọi endpoint /release trên dataserver để thông báo giải phóng
                using (HttpClient httpClient = new HttpClient())
                {
                    var releasePayload = JsonConvert.SerializeObject(new { client_id = req.client_id });
                    var releaseContent = new StringContent(releasePayload, Encoding.UTF8, "application/json");
                    try
                    {
                        var releaseResponse = await httpClient.PostAsync(releasedServer.Url + "/release", releaseContent);
                        AddNotification($"Đã thông báo giải phóng cho {releasedServer.Name}.", "info");
                    }
                    catch (Exception ex)
                    {
                        AddNotification("Lỗi thông báo giải phóng cho Dataserver: " + ex.Message, "error");
                    }
                }
                await WriteResponse(context, JsonConvert.SerializeObject(new { status = "success", message = "Access released successfully" }));
            }
            else
            {
                context.Response.StatusCode = 404;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "No server found for this client" }));
            }
        }



        // Hàm trả về JSON trạng thái của các server
        private string GetServerStatusJson()
        {
            var serverList = new List<object>();
            var statusDict = new Dictionary<string, object>();
            lock (stateLock)
            {
                foreach (var server in servers)
                {
                    serverList.Add(new { id = server.Id, name = server.Name, url = server.Url });
                    statusDict[server.Id.ToString()] = new
                    {
                        busy = server.Busy,
                        current_client = server.CurrentClient,
                        last_access = server.LastAccess == DateTime.MinValue ? "" : server.LastAccess.ToString("HH:mm:ss")
                    };
                }
            }
            var result = new
            {
                servers = serverList,
                status = statusDict
            };
            return JsonConvert.SerializeObject(result);
        }

        // Ghi response về cho client
        private async Task WriteResponse(HttpListenerContext context, string responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            using (var output = context.Response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private async Task ReleaseAllClientsAsync()
        {
            List<DatabaseServer> serversToRelease;
            // Lấy danh sách các server có client (có thể là rỗng)
            lock (stateLock)
            {
                serversToRelease = servers.Where(s => !string.IsNullOrWhiteSpace(s.CurrentClient)).ToList();
                // Reset trạng thái của các server đó
                foreach (var s in serversToRelease)
                {
                    s.Busy = false;
                    s.CurrentClient = "";
                    s.LastAccess = DateTime.Now;
                }
            }

            // Thông báo giải phóng tới từng dataserver thông qua endpoint /release
            foreach (var server in serversToRelease)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Ở đây, vì hệ thống thiết kế /release yêu cầu client_id trùng với currentClient,
                    // chúng ta có thể gửi một payload mặc định (ví dụ "force_release") nếu dataserver đã reset currentClient.
                    var releasePayload = JsonConvert.SerializeObject(new { client_id = "force_release" });
                    var releaseContent = new StringContent(releasePayload, Encoding.UTF8, "application/json");
                    try
                    {
                        var releaseResponse = await httpClient.PostAsync(server.Url + "/release", releaseContent);
                        if (releaseResponse.IsSuccessStatusCode)
                        {
                            AddNotification($"Đã giải phóng {server.Name}.", "info");
                        }
                        else
                        {
                            AddNotification($"Lỗi khi giải phóng {server.Name}: {releaseResponse.ReasonPhrase}", "error");
                        }
                    }
                    catch (Exception ex)
                    {
                        AddNotification($"Lỗi khi giải phóng {server.Name}: {ex.Message}", "error");
                    }
                }
            }
        }



        private void CoordinatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dừng HttpListener khi đóng form
            cts.Cancel();
            if (httpListener != null && httpListener.IsListening)
            {
                httpListener.Stop();
            }

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        // Nút "Kết nối lại" – ví dụ chỉ hiển thị thông báo
        private void btnReconnect_Click(object sender, EventArgs e)
        {
            AddNotification("Đã cập nhật URL: " + txtCoordinatorUrl.Text.Trim(), "success");
        }
    }

    // Các lớp mô tả request JSON
    public class RequestAccessRequest
    {
        public string client_id { get; set; }
    }

    public class ReleaseAccessRequest
    {
        public string client_id { get; set; }
    }

    // Lớp mô tả database server
    public class DatabaseServer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool Busy { get; set; }
        public string CurrentClient { get; set; }
        public DateTime LastAccess { get; set; }
    }

}