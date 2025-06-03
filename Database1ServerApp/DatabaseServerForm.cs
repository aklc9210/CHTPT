using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Database1ServerApp
{
    public partial class DatabaseServerForm : DevExpress.XtraEditors.XtraForm
    {
        private HttpListener httpListener;
        private CancellationTokenSource cts;
        private readonly int serverId;
        private readonly int port;

        private List<Dictionary<string, string>> sampleData;
        private string currentClient = "";
        public DatabaseServerForm(int serverId, int port)
        {
            InitializeComponent();
            this.serverId = serverId;
            this.port = port;
            lblServerInfo.Text = $"Database Server {serverId} (Port {port})";
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            sampleData = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>{{"Item","Item 1"}, {"Value","Value 1"}},
                new Dictionary<string, string>{{"Item","Item 2"}, {"Value","Value 2"}},
                new Dictionary<string, string>{{"Item","Item 3"}, {"Value","Value 3"}},
                new Dictionary<string, string>{{"Item","Item 4"}, {"Value",$"Value 4 from Server {serverId}"}},
                new Dictionary<string, string>{{"Item","Item 5"}, {"Value",$"Value 5 from Server {serverId}"}}
            };
        }

        private void DatabaseServerForm_Load(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();
            Task.Run(() => StartHttpListener(cts.Token));
            AddLog("Database Server 1 khởi động.");
        }

        private void DatabaseServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(currentClient))
            {
                // Nếu đang có client truy cập → hiển thị cảnh báo và hủy đóng form
                MessageBox.Show(
                    $"Không thể tắt DataServer {serverId} vì đang có client {currentClient} đang truy cập.",
                    "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                e.Cancel = true; // Ngăn việc đóng cửa sổ
                return;
            }

            cts.Cancel();
            if (httpListener != null && httpListener.IsListening)
                httpListener.Stop();
        }

        // Các endpoint: /notify_access, /data, /release
        private async Task StartHttpListener(CancellationToken token)
        {
            httpListener = new HttpListener();
            string listenUrl;
            listenUrl = $"http://+:{port}/";
            httpListener.Prefixes.Add(listenUrl);
            try
            {
                httpListener.Start();
                AddLog("HttpListener khởi động tại " + listenUrl);
            }
            catch (Exception ex)
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
                        AddLog("Lỗi listener: " + ex.Message);
                }
            }
        }

        private async void ProcessRequest(HttpListenerContext context)
        {
            string responseString = "";
            int statusCode = 200;
            try
            {
                string rawUrl = context.Request.RawUrl;
                string method = context.Request.HttpMethod;

                if (rawUrl.StartsWith("/server_status") && method == "GET")
                {
                    var respObj = new { status = "ok", server_id = serverId };
                    string respJson = JsonConvert.SerializeObject(respObj);

                    context.Response.StatusCode = 200;
                    context.Response.ContentType = "application/json";
                    byte[] buffer1 = Encoding.UTF8.GetBytes(respJson);  // <-- trùng tên buffer
                    context.Response.ContentLength64 = buffer1.Length;
                    await context.Response.OutputStream.WriteAsync(buffer1, 0, buffer1.Length);
                    context.Response.Close();
                    return;
                }

                if (rawUrl.StartsWith("/notify_access") && method == "POST")
                {
                    await HandleNotifyAccess(context);
                    return;
                }
                else if (rawUrl.StartsWith("/data") && method == "GET")
                {
                    responseString = GetDataJson(context);
                }
                else if (rawUrl.StartsWith("/release") && method == "POST")
                {
                    await HandleRelease(context);
                    return;
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
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            using (var output = context.Response.OutputStream)
            {
                await output.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        // Xử lý /notify_access: nhận thông báo từ Coordinator, cập nhật currentClient
        private async Task HandleNotifyAccess(HttpListenerContext context)
        {
            string body;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }

            dynamic req = JsonConvert.DeserializeObject(body);

            if (req == null || req.client_id == null)
            {
                context.Response.StatusCode = 400;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "Client ID is required" }));
                return;
            }
            currentClient = req.client_id;
            AddLog($"Client {currentClient} đang truy cập.");
            var resp = new { status = "success", message = $"Server {serverId} sẵn sàng cho client {currentClient}" };
            await WriteResponse(context, JsonConvert.SerializeObject(resp));
        }

        // Xử lý /data: chỉ cho phép nếu header X-Client-ID khớp với currentClient
        private string GetDataJson(HttpListenerContext context)
        {
            string clientId = context.Request.Headers["X-Client-ID"];
            if (string.IsNullOrWhiteSpace(clientId) || clientId != currentClient)
            {
                context.Response.StatusCode = 403;
                return JsonConvert.SerializeObject(new { error = "Unauthorized access. This client was not assigned to this server." });
            }
            AddLog($"Client {clientId} truy xuất dữ liệu.");
            return JsonConvert.SerializeObject(new { server_id = serverId, data = sampleData, timestamp = DateTime.Now.ToString("O") });
        }

        // Xử lý /release: giải phóng currentClient
        private async Task HandleRelease(HttpListenerContext context)
        {
            string body;
            using (var reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
            {
                body = await reader.ReadToEndAsync();
            }
            dynamic req = JsonConvert.DeserializeObject(body);
            if (req == null || req.client_id == null)
            {
                context.Response.StatusCode = 400;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "Client ID is required" }));
                return;
            }
            string clientId = req.client_id;
            if (clientId != currentClient)
            {
                context.Response.StatusCode = 403;
                await WriteResponse(context, JsonConvert.SerializeObject(new { error = "Unauthorized access. This client was not assigned to this server." }));
                return;
            }
            AddLog($"Client {clientId} đã giải phóng quyền truy cập.");
            currentClient = "";
            var resp = new { status = "success", message = $"Access released for client {clientId}" };
            await WriteResponse(context, JsonConvert.SerializeObject(resp));
        }

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

        private void AddLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => AddLog(message)));
                return;
            }
            lstLogs.Items.Insert(0, $"[{DateTime.Now:T}] {message}");
            lblCurrentClient.Text = string.IsNullOrWhiteSpace(currentClient) ? "Không có client" : $"Client: {currentClient}";
        }


    }
}