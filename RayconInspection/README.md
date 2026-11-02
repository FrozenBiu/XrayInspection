# Raycon X-Ray Remote Inspection Dashboard

Hệ thống Dashboard theo dõi và quản lý ảnh sản phẩm lỗi (Reject) thời gian thực dành cho máy soi tia X công nghiệp **Sesotec Raycon EX2**.

---

## 1. Điểm Cải Tiến Mới Nhất

* ✅ **Khắc phục triệt để lỗi `ECONNREFUSED` khi khởi động**:
  * Backend API nạp cổng 5000 siêu tốc (dưới 0.5s) nhờ cơ chế quét ảnh nền bất đồng bộ.
  * Trình khởi động tự động thăm dò cổng 5000, **chờ Backend sẵn sàng 100% rồi mới bật Frontend và mở trình duyệt**.
  * Frontend có cơ chế tự động thử lại kết nối mượt mà.
* ✅ **Tích hợp Quản lý Khay Hệ thống (System Tray Manager)**:
  * Gom toàn bộ 2 cửa sổ terminal thành **1 tiến trình duy nhất chạy ngầm trong System Tray** (icon hình chiếc khiên gần đồng hồ Windows).
  * Không còn cửa sổ đen xì choáng Taskbar.
  * Nhấp chuột phải vào icon khay hệ thống để: **Mở Dashboard**, **Xem Log**, **Khởi động lại**, hoặc **Thoát hoàn toàn**.
  * Nhấp đúp chuột vào icon để mở nhanh trình duyệt `http://localhost:5173`.
* ✅ **Thêm Hướng dẫn Cấu hình Trực quan trên Giao diện**:
  * Trang **Cài đặt Hệ thống (Settings)** có sẵn thẻ hướng dẫn từng bước và form nhập đường dẫn trực tiếp, tự động lưu vào `appsettings.json`.

---

## 2. Cách Khởi động

### Cách 1: Chế độ Khay Hệ Thống (Khuyên dùng - Cực gọn gàng)
Nhấp đúp chuột vào file:
👉 **`start.bat`**
*Cửa sổ sẽ tự động ẩn đi và xuất hiện một biểu tượng hình khiên nhỏ ở góc dưới bên phải (gần đồng hồ Windows), đồng thời tự mở Dashboard trên trình duyệt.*

### Cách 2: Chế độ Console (Xem log trực tiếp nếu cần debug)
Nhấp đúp chuột vào file:
👉 **`start_with_console.bat`**

---

## 3. Cấu trúc Các Trang Chức năng trên Dashboard

1. 📊 **Tổng quan (Overview Dashboard)**:
   * KPI: Tổng số ảnh lỗi, số lượng dị vật, khuyết tật bao bì, thông số phát tia X (kV / mA).
   * Phân bổ tỷ lệ lỗi theo sản phẩm và xem trước 6 ảnh lỗi mới nhất.
2. 🔬 **Soi ảnh Trực tiếp (Live Inspection)**:
   * **Gallery Viewer**: Zoom tới 600%, Pan kéo di chuyển, chỉnh Độ sáng & Tương phản tia X, chế độ Đảo màu âm bản.
   * **Metadata Panel**: Hiển thị điện áp, dòng phát, tốc độ băng tải, thời gian.
   * **Filmstrip Bar**: Dải thumbnail realtime cuộn ngang.
3. 📁 **Thư viện & Lịch sử (History & Archive)**:
   * Lọc theo Tên sản phẩm, lọc theo Ngày/Tháng/Năm (Date picker), lọc theo loại dị vật.
   * Chế độ xem Lưới (Grid) hoặc Bảng (Table), xem nhanh ảnh (Quick View).
4. ⚙️ **Cài đặt Hệ thống (Settings)**:
   * Nhập đường dẫn thư mục `WatchDirectory` trực tiếp từ giao diện, bấm nút **Kiểm Tra Thử** và **Lưu & Áp Dụng Ngay**.
