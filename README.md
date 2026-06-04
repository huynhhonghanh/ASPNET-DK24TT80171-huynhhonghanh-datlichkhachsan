# XÂY DỰNG WEBSITE ĐẶT PHÒNG KHÁCH SẠN VEN BIỂN VŨNG TÀU

| | |
|---|---|
| **Sinh viên** | Huỳnh Hồng Hạnh |
| **Mã lớp** | DK24TT80171 |
| **Đề tài** | Xây dựng website đặt phòng khách sạn ven biển Vũng Tàu |

---

## Giới thiệu

Website **Vũng Tàu Booking** giúp khách hàng tìm và đặt phòng khách sạn ven biển; quản trị viên quản lý khách sạn, phòng, đơn đặt và tài khoản.

**Công nghệ:** ASP.NET Core MVC (.NET 8), Entity Framework Core, SQL Server, Bootstrap 5.

---

## Cấu trúc thư mục

```
khachsan/
├── README.md              ← File này
├── đồ án.md               ← Báo cáo chi tiết
└── src/                   ← Mã nguồn website
    ├── backup-database.ps1
    └── backups/           ← File .bak, .sql, .zip (sau khi chạy backup)
```

---

## Cài đặt và chạy

**Yêu cầu:** .NET 8 SDK, SQL Server.

1. Mở thư mục `src`, copy `appsettings.example.json` thành `appsettings.json` và sửa connection string.
2. Chạy:

```powershell
cd src
dotnet restore
dotnet build
dotnet run
```

3. Mở trình duyệt: `http://localhost:5110`

**Tài khoản demo:**

| Vai trò | Tên đăng nhập | Mật khẩu |
|---------|---------------|----------|
| Admin | admin | Admin@123 |
| Khách | khach1 | 123456 |
| Khách | khach2 | 123456 |

---

## Backup database (nộp bài)

```powershell
cd src
.\backup-database.ps1
```

Kết quả trong `src\backups\`: file `.bak`, `.sql` và `.zip` — gửi kèm cho giảng viên.

**Khôi phục .bak:** SQL Server Management Studio → Restore Database.

---

## Publish (tùy chọn)

```powershell
cd src
dotnet publish DatPhongKhachSanVungTau.csproj -c Release -o ..\publish
```

---

*Báo cáo đầy đủ: xem file `đồ án.md`.*
