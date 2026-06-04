# XÂY DỰNG WEBSITE ĐẶT PHÒNG KHÁCH SẠN VEN BIỂN VŨNG TÀU

| | |
|---|---|
| **Sinh viên** | Huỳnh Hồng Hạnh |
| **Mã lớp** | DK24TT80171 |

Website đặt phòng khách sạn ven biển Vũng Tàu — ASP.NET Core MVC (.NET 8) + SQL Server.

## Chạy nhanh

```powershell
dotnet restore
dotnet run
```

Sửa `appsettings.json` trước khi chạy. Truy cập: `http://localhost:5110`

| Vai trò | User | Password |
|---------|------|----------|
| Admin | admin | Admin@123 |
| Khách | khach1 | 123456 |

## Backup DB

```powershell
.\backup-database.ps1
```

File trong thư mục `backups\` (.bak, .sql, .zip).

*Báo cáo: `../đồ án.md` · Hướng dẫn đầy đủ: `../README.md`*
