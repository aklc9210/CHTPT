# Data Access System - Client-Server Architecture

## Giới thiệu

Hệ thống quản lý truy cập dữ liệu này cho phép các client gửi yêu cầu truy cập vào các server cơ sở dữ liệu (Database Servers). Hệ thống sử dụng một **Coordinator** để phân phối yêu cầu truy cập giữa các server và xử lý các lệnh giải phóng quyền truy cập từ client. Dự án mô phỏng một môi trường quản lý kết nối giữa client và server, bao gồm:

- **ClientApp**: Ứng dụng cho phép người dùng tạo ClientID và gửi yêu cầu truy cập.
- **CoordinatorApp**: Ứng dụng điều phối, quản lý trạng thái của các server và phân phối truy cập cho client.
- **DatabaseServerApp**: Ứng dụng mô phỏng các server cơ sở dữ liệu, nhận và xử lý yêu cầu truy cập từ client thông qua Coordinator.

## Luồng hoạt động

### 1. Khởi tạo ClientID
- Người dùng tạo ClientID mới thông qua nút "Tạo ClientID" trong ClientApp.
- ClientID này sẽ được dùng cho các yêu cầu truy cập sau này.

### 2. Yêu cầu truy cập
- Sau khi có ClientID, người dùng bấm nút "Yêu cầu truy cập".
- ClientApp gửi yêu cầu (POST) tới CoordinatorApp, kèm theo `client_id`.
- CoordinatorApp kiểm tra trạng thái các Database Servers. Nếu có server chưa có client kết nối, Coordinator sẽ phân phối server cho client và thông báo cho server đó.
- Client sẽ truy xuất dữ liệu từ server đã được phân phối.

### 3. Giải phóng quyền truy cập
- Khi không còn cần thiết, client bấm nút "Giải phóng quyền".
- Coordinator nhận thông báo giải phóng và cập nhật lại trạng thái của server, cho phép client khác kết nối.
- Server cũng nhận thông báo từ Coordinator để giải phóng tài nguyên.

### 4. Lịch sử và Trạng thái
- Coordinator hiển thị trạng thái của các server (bận hay rảnh) và thông tin client đang truy cập.
- Dữ liệu và trạng thái được tự động cập nhật thông qua timer hoặc yêu cầu từ client.

## Cấu hình và Cài đặt

### Yêu cầu

Để chạy hệ thống, cần cài đặt các công cụ và phần mềm sau:

- **.NET Framework 4.8** trở lên.
- **DevExpress** (cho giao diện người dùng).
- **Newtonsoft.Json** (để xử lý JSON).

### Cấu hình

1. **CoordinatorApp**  
   Trong file `CoordinatorForm.cs`, cập nhật cấu hình địa chỉ IP và port của các server. Ví dụ:
   ```csharp
   new DatabaseServer
   {
       Id = 1,
       Name = "Database Server 1",
       Url = "http://192.168.214.103:5001",
       Busy = false,
       CurrentClient = "",
       LastAccess = DateTime.MinValue
   }
   ```
2. **ClientApp**  
   Trong file `ClientForm.cs`, cập nhật địa chỉ của Coordinator (mặc định: `http://192.168.214.103:5000`). Nếu chạy trên môi trường khác, hãy thay đổi IP của máy chủ.
3. **Database Server**  
   Trong file `DatabaseServerForm.cs` hoặc `Database2ServerForm.cs`, cấu hình URL và port cho từng server cơ sở dữ liệu, ví dụ:
   ```csharp
   string listenUrl = $"http://+:{port}/";
   ```

### Cài đặt

- **Clone repository:**
  ```bash
  git clone https://github.com/yourusername/your-repo.git
  ```
- **Cài đặt các gói NuGet:**  
  Trong Visual Studio, chạy các lệnh sau:
  ```bash
  Install-Package Newtonsoft.Json
  Install-Package Microsoft.AspNet.SignalR.Client
  ```
- **Chạy các ứng dụng:**  
  - **CoordinatorApp:** Chạy ứng dụng để bắt đầu lắng nghe và phân phối yêu cầu.
  - **DatabaseServerApp:** Chạy từng instance của ứng dụng, mỗi instance mô phỏng một server cơ sở dữ liệu.
  - **ClientApp:** Chạy ứng dụng và tạo ClientID để gửi yêu cầu truy cập.

## Các Endpoint Chính

Dưới đây là danh sách các endpoint giao tiếp giữa các ứng dụng:

- **POST /request_access**  
  Gửi yêu cầu truy cập từ client tới Coordinator.
  ```json
  {
    "client_id": "client123"
  }
  ```
  Phản hồi:
  ```json
  {
    "server_id": 1,
    "server_name": "Database Server 1",
    "server_url": "http://192.168.214.103:5001"
  }
  ```

- **POST /release_access**  
  Giải phóng quyền truy cập của client từ Coordinator.

- **GET /server_status**  
  Lấy thông tin trạng thái của tất cả các server.

- **POST /notify_access**  
  Nhận thông báo từ Coordinator về client đang truy cập server.

- **GET /data**  
  Lấy dữ liệu từ server (yêu cầu header `X-Client-ID` trùng với ClientID).

- **POST /release**  
  Giải phóng quyền truy cập của client trên server.

- **Ví dụ về yêu cầu GET /data:**  
  Thêm header:
  ```http
  X-Client-ID: client123
  ```
  Phản hồi:
  ```json
  {
    "server_id": 1,
    "data": [{"Item": "Item 1", "Value": "Value 1"}],
    "timestamp": "2023-04-02T15:30:00"
  }
  ```

## Các Lỗi Thường Gặp

- **503 Service Unavailable:** Tất cả các server đang bận.
- **409 Conflict:** Client đã kết nối tới server.
- **403 Forbidden:** Client truy cập không hợp lệ.
- **500 Internal Server Error:** Lỗi khi xử lý yêu cầu.

Dựa trên việc xem qua các file code của dự án, ta có thể mô tả thêm về những công nghệ chính cũng như luồng hoạt động của chúng như sau:

---

## Các Công Nghệ Sử Dụng

- **.NET Framework & C#**  
  Dự án được xây dựng trên nền tảng .NET Framework sử dụng ngôn ngữ C#. Các ứng dụng của Coordinator, Database Server và Client đều được phát triển dưới dạng Windows Forms (WinForms) nhằm tạo giao diện người dùng trực quan. 

- **DevExpress**  
  Các giao diện chính được xây dựng bằng DevExpress – một bộ thư viện UI mạnh mẽ giúp tạo các form hiện đại và dễ sử dụng. Điều này được thể hiện rõ qua các file như CoordinatorForm.cs và ClientForm.cs. 

- **HttpListener & RESTful API**  
  Các ứng dụng Coordinator và Database Server sử dụng `HttpListener` để mở cổng HTTP và lắng nghe các yêu cầu từ client. Các endpoint RESTful như `/request_access`, `/release_access`, `/notify_access`, `/data` và `/release` được định nghĩa để xử lý việc yêu cầu cấp, truy xuất dữ liệu và giải phóng kết nối. 

- **Newtonsoft.Json**  
  Việc chuyển đổi giữa đối tượng C# và định dạng JSON được thực hiện thông qua thư viện Newtonsoft.Json. Điều này giúp trao đổi dữ liệu giữa các thành phần của hệ thống trở nên dễ dàng và chuẩn hóa. (citeturn1file0, citeturn1file6)

- **Asynchronous Programming & Multithreading**  
  Các thao tác bất đồng bộ (async/await) được sử dụng để xử lý các request HTTP và thực hiện cập nhật trạng thái trong nền, đảm bảo giao diện người dùng luôn phản hồi nhanh. Đồng thời, việc sử dụng `Task.Run` và `CancellationToken` giúp quản lý đa luồng khi lắng nghe và xử lý các request từ HttpListener.

- **Socket.IO (Dashboard Web)**  
  Mặc dù phần giao diện dashboard được xây dựng bằng HTML/Bootstrap và sử dụng Socket.IO để cập nhật trạng thái thời gian thực, nhưng phần code chủ yếu của dự án vẫn tập trung vào giao tiếp qua RESTful API giữa các ứng dụng client, coordinator và database server.

---

## Luồng Hoạt Động Của Hệ Thống

1. **Khởi Động Hệ Thống:**
   - **Coordinator:** Khi khởi động, CoordinatorForm thiết lập danh sách các Database Server và bắt đầu HttpListener để lắng nghe các request từ client. Hệ thống sẽ liên tục cập nhật trạng thái của các server (đang bận hay sẵn sàng) và hiển thị thông báo trên dashboard. 
   - **Database Server:** Mỗi database server (DatabaseServerForm và Database2ServerForm) khởi tạo HttpListener trên cổng tương ứng và load dữ liệu mẫu. Server sẽ lắng nghe các endpoint như `/notify_access`, `/data` và `/release` để xử lý các yêu cầu từ Coordinator và Client.
   - **Client:** ClientForm cho phép người dùng tạo ClientID, nhập địa chỉ Coordinator và gửi yêu cầu truy cập tới hệ thống. 

2. **Yêu Cầu Truy Cập:**
   - Client gửi một request POST đến endpoint `/request_access` của Coordinator, kèm theo thông tin ClientID.
   - Coordinator nhận request, kiểm tra trạng thái các Database Server (đã có client nào kết nối chưa, server có đang bận hay không). Nếu client đã được kết nối với một server thì trả về thông báo lỗi (409 Conflict); nếu tất cả server đang bận thì trả về mã lỗi 503.
   - Nếu có server rảnh, Coordinator sẽ chọn server dựa trên thuật toán (chọn server có thời gian LastAccess nhỏ nhất) và cập nhật trạng thái của server đó, sau đó gọi endpoint `/notify_access` trên Database Server để thông báo về client sắp kết nối.

3. **Truy Xuất Dữ Liệu:**
   - Sau khi nhận thông báo từ Coordinator, Client sẽ lấy thông tin của Database Server (server_id, server_name, server_url).
   - Client sử dụng HttpClient để gửi yêu cầu GET đến endpoint `/data` của Database Server, kèm header “X-Client-ID” xác thực.
   - Database Server kiểm tra ClientID trong header so với client đã được gán (currentClient). Nếu khớp, server trả về dữ liệu mẫu kèm theo timestamp; nếu không, trả về lỗi 403 Forbidden. 

4. **Giải Phóng Quyền Truy Cập:**
   - Khi client không cần sử dụng dữ liệu nữa, nó sẽ gửi request POST đến endpoint `/release_access` của Coordinator.
   - Coordinator tìm kiếm và giải phóng server đang được gán cho client đó, cập nhật trạng thái (đánh dấu server là rảnh) và gọi endpoint `/release` trên Database Server để thông báo giải phóng.
   - Sau khi giải phóng, trạng thái mới của các server được cập nhật và hiển thị lại trên dashboard của Coordinator.

5. **Cập Nhật và Giám Sát:**
   - Các ứng dụng sử dụng Timer và đa luồng để cập nhật trạng thái server theo thời gian thực. Coordinator thường xuyên refresh dữ liệu hiển thị trên DataGridView và gửi thông báo cho người dùng qua danh sách thông báo.
   - Dashboard web sử dụng Socket.IO để nhận các thông báo và cập nhật giao diện cho người dùng trong thời gian thực.


