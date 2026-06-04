# XÂY DỰNG WEBSITE ĐẶT PHÒNG KHÁCH SẠN VEN BIỂN VŨNG TÀU

| | |
|---|---|
| **Sinh viên** | Huỳnh Hồng Hạnh |
| **Mã lớp** | DK24TT80171 |
| **Đề tài** | Xây dựng website đặt phòng khách sạn ven biển Vũng Tàu |

---

## Cấu trúc thư mục (theo quy định đồ án)

```
khachsan/
├── README.md
├── src/                 ← Mã nguồn ASP.NET Core MVC
├── thesis/              ← Báo cáo (doc, pdf)
└── setup/               ← Script, dữ liệu mẫu (backup DB)
    └── dulieu/
```

---

## Chạy website

```powershell
cd src
dotnet restore
dotnet run
```

Sửa `src/appsettings.json` (copy từ `appsettings.example.json`). Truy cập: `http://localhost:5110`

| Vai trò | User | Password |
|---------|------|----------|
| Admin | admin | Admin@123 |
| Khách | khach1 | 123456 |

---

## Backup database

```powershell
cd src
.\backup-database.ps1
```

---

## Git (repo ở thư mục gốc `khachsan`)

```powershell
cd C:\Users\Admin\Desktop\khachsan
git status
git add .
git commit -m "Mo ta"
git push
```
