# Hướng dẫn Kết nối Ứng dụng Inspection với Máy X-Ray Raycon EX2

Tài liệu này hướng dẫn chi tiết cách cấu hình mạng, chia sẻ thư mục trên máy X-Ray và thiết lập ứng dụng Inspection để tự động lấy ảnh sản phẩm lỗi theo thời gian thực.

---

## 1. Cấu hình trên Máy tính của Máy X-Ray (HMI PC)

Máy X-Ray Raycon EX2 chạy hệ điều hành Windows (thường là Windows 7 hoặc Windows 10 IoT/Embedded). Để ứng dụng từ xa có thể lấy ảnh mà không làm ảnh hưởng đến hoạt động soi của máy:

### Bước 1.1: Xác định địa chỉ IP của Máy X-Ray

1. Trên màn hình máy X-Ray, thoát ra ngoài Windows (hoặc cắm bàn phím nhấn tổ hợp phím `Ctrl + Esc` / `Windows + R`).
2. Mở Command Prompt (`cmd`) và gõ lệnh:
   ```cmd
   ipconfig
   ```
3. Ghi lại địa chỉ IPv4 của card mạng LAN nối vào mạng nhà máy (ví dụ: `192.168.1.150`).

### Bước 1.2: Chia sẻ thư mục `Archive\Image\Reject` qua Mạng LAN

1. Tìm đến thư mục chạy của phần mềm X-Ray, thông thường nằm tại:
   `C:\Sesotec\GUI_XRS\Archive\Image\Reject\` (hoặc thư mục tương ứng trong máy của bạn).
2. Nhấp chuột phải vào thư mục **`Reject`** $\rightarrow$ chọn **Properties** $\rightarrow$ chọn thẻ **Sharing**.
3. Nhấp vào nút **Advanced Sharing...** $\rightarrow$ Tích chọn **Share this folder**.
4. Đặt tên Share Name: `RayconRejectImages`.
5. Nhấp vào nút **Permissions**:
   - Chỉ cấp quyền **Read** (Đọc) cho nhóm người dùng `Everyone` hoặc tài khoản riêng của nhà máy (tuyệt đối không cấp quyền Write hay Full Control để bảo vệ tính toàn vẹn của dữ liệu máy).
6. Nhấp **OK** để hoàn tất chia sẻ.

---

## 2. Kiểm tra Kết nối từ Máy tính Kỹ sư / Máy chủ Ứng dụng

Trước khi chạy ứng dụng Inspection, hãy đảm bảo máy tính của bạn đã thông mạng với máy X-Ray:

1. Trên máy tính của bạn, nhấn phím `Windows + R`.
2. Gõ đường dẫn mạng dạng:
   ```text
   \\192.168.1.150\RayconRejectImages
   ```
3. Nhấn **Enter**:
   - Nếu cửa sổ File Explorer hiện ra danh sách các file có đuôi `.xri` (dạng `2026-10-31_10-29-37,77.xri`), bạn đã kết nối mạng thành công 100%!

---

## 3. Cấu hình Ứng dụng Inspection Service

Trong mã nguồn của ứng dụng Backend, bạn sẽ mở file cấu hình `appsettings.json`:

```json
{
  "RayconInspection": {
    "MachineId": "RAYCON_EX2_LINE_01",
    "MachineName": "Máy Soi Tia X Dây Chuyền 1",

    // Đường dẫn mạng tới thư mục Reject của máy X-Ray:
    "WatchDirectory": "\\\\192.168.1.150\\RayconRejectImages",

    // HOẶC nếu chạy test ngay trên máy tính của bạn:
    // "WatchDirectory": "E:\\VisualStudioCode\\MyProjects\\RayconEX2\\Source\\GUI_XRS\\Archive\\Image\\Reject",

    "CacheOutputDirectory": "./wwwroot/cached_images",
    "FileFilter": "*.xri",
    "ScanIntervalMs": 100
  }
}
```

---

## 4. Kiểm tra Thử nghiệm Ngay Lập tức (Test Run)

Ngay trong thư mục dự án của bạn hiện tại đã có sẵn **50 file `.xri` thực tế** được lưu tại:
`e:\VisualStudioCode\MyProjects\RayconEX2\Source\GUI_XRS\Archive\Image\Reject\`

### Cách chạy thử chuyển đổi 1 file `.xri` sang `.png`:

Khi khởi động ứng dụng Inspection, service sẽ:

1. Quét toàn bộ các file `.xri` hiện có trong thư mục để nạp danh sách lịch sử ban đầu.
2. Dùng hàm `SaveAsCombinedPng()` trong `XrsImage.dll` để sinh file `.png` (ảnh xám tia X kết hợp khoanh vùng đánh dấu dị vật).
3. Bật `FileSystemWatcher` ở chế độ thường trực.
4. Mỗi khi dây chuyền phát hiện lỗi và máy X-Ray nhả 1 file `.xri` mới, giao diện **Filmstrip & Gallery** sẽ lập tức hiện thẻ ảnh lỗi lên màn hình mà không cần tải lại trang.
