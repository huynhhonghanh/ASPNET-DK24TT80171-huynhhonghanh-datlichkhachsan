using DatPhongKhachSanVungTau.Models;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Data;

public static class DbInitializer
{
    public static void KhoiTaoDuLieu(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Tự tạo DB + áp dụng migration nếu chưa có
        db.Database.Migrate();

        // Nếu đã có dữ liệu thì không seed lại
        if (db.NguoiDung.Any())
        {
            return;
        }

        // 1) Người dùng
        var admin = new NguoiDung
        {
            TenDangNhap = "admin",
            MatKhau = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            HoTen = "Quản trị viên",
            Email = "admin@vungtaubooking.local",
            SoDienThoai = "0900000000",
            CCCD = "000000000000",
            DiaChi = "Vũng Tàu",
            VaiTro = HangSo.VaiTroAdmin,
            TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanHoatDong
        };

        var khach1 = new NguoiDung
        {
            TenDangNhap = "khach1",
            MatKhau = BCrypt.Net.BCrypt.HashPassword("123456"),
            HoTen = "Nguyễn Văn Khách",
            Email = "khach1@example.com",
            SoDienThoai = "0911111111",
            CCCD = "012345678901",
            DiaChi = "TP. Hồ Chí Minh",
            VaiTro = HangSo.VaiTroKhachHang,
            TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanHoatDong
        };

        var khach2 = new NguoiDung
        {
            TenDangNhap = "khach2",
            MatKhau = BCrypt.Net.BCrypt.HashPassword("123456"),
            HoTen = "Trần Thị Biển",
            Email = "khach2@example.com",
            SoDienThoai = "0922222222",
            CCCD = "012345678902",
            DiaChi = "Bà Rịa - Vũng Tàu",
            VaiTro = HangSo.VaiTroKhachHang,
            TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanHoatDong
        };

        db.NguoiDung.AddRange(admin, khach1, khach2);

        // 2) Khách sạn (Vũng Tàu, theo bãi biển)
        var ksPullman = new KhachSan
        {
            TenKhachSan = "Pullman Vũng Tàu",
            KhuVuc = "Bãi Sau",
            DiaChiChiTiet = "15 Thùy Vân, Phường 8, Vũng Tàu",
            MoTa = "Khách sạn 5 sao ven biển, phù hợp nghỉ dưỡng cao cấp.",
            HinhAnh = "/images/khachsan/pullman.svg",
            XepHang = 5,
            KhoangCachBien = 50
        };
        var ksImperial = new KhachSan
        {
            TenKhachSan = "Imperial Hotel",
            KhuVuc = "Bãi Trước",
            DiaChiChiTiet = "159 Thùy Vân, Phường Thắng Tam, Vũng Tàu",
            MoTa = "Phong cách sang trọng, vị trí trung tâm, tiện di chuyển.",
            HinhAnh = "/images/khachsan/imperial.svg",
            XepHang = 5,
            KhoangCachBien = 80
        };
        var ksMalibu = new KhachSan
        {
            TenKhachSan = "Malibu Hotel",
            KhuVuc = "Bãi Sau",
            DiaChiChiTiet = "263 Lê Hồng Phong, Phường 8, Vũng Tàu",
            MoTa = "Khách sạn hiện đại, nhiều phòng view biển.",
            HinhAnh = "/images/khachsan/malibu.svg",
            XepHang = 4,
            KhoangCachBien = 200
        };
        var ksSammy = new KhachSan
        {
            TenKhachSan = "Sammy Hotel",
            KhuVuc = "Bãi Trước",
            DiaChiChiTiet = "157 Thùy Vân, Phường Thắng Tam, Vũng Tàu",
            MoTa = "Khách sạn 4 sao, phù hợp gia đình và công tác.",
            HinhAnh = "/images/khachsan/sammy.svg",
            XepHang = 4,
            KhoangCachBien = 120
        };
        var ksCapSaintJacques = new KhachSan
        {
            TenKhachSan = "Cap Saint Jacques",
            KhuVuc = "Bãi Dâu",
            DiaChiChiTiet = "169 Thùy Vân, Phường 8, Vũng Tàu",
            MoTa = "Khu nghỉ dưỡng thoáng mát, gần biển, yên tĩnh.",
            HinhAnh = "/images/khachsan/cap-saint-jacques.svg",
            XepHang = 3,
            KhoangCachBien = 150
        };
        var ksLanRung = new KhachSan
        {
            TenKhachSan = "Lan Rừng Resort",
            KhuVuc = "Bãi Dứa",
            DiaChiChiTiet = "3-6 Hạ Long, Phường 2, Vũng Tàu",
            MoTa = "Resort phong cách châu Âu, nhiều góc sống ảo.",
            HinhAnh = "/images/khachsan/lan-rung.svg",
            XepHang = 4,
            KhoangCachBien = 60
        };

        db.KhachSan.AddRange(ksPullman, ksImperial, ksMalibu, ksSammy, ksCapSaintJacques, ksLanRung);

        // 3) Loại phòng
        var lpDon = new LoaiPhong { TenLoaiPhong = "Phòng đơn", SoNguoiToiDa = 1, MoTa = "Phòng cho 1 người, tiện nghi cơ bản." };
        var lpDoi = new LoaiPhong { TenLoaiPhong = "Phòng đôi", SoNguoiToiDa = 2, MoTa = "Phòng cho 2 người, phù hợp cặp đôi." };
        var lpVip = new LoaiPhong { TenLoaiPhong = "Phòng VIP", SoNguoiToiDa = 2, MoTa = "Không gian rộng, nội thất cao cấp." };
        var lpGiaDinh = new LoaiPhong { TenLoaiPhong = "Phòng gia đình", SoNguoiToiDa = 4, MoTa = "Phù hợp nhóm bạn và gia đình." };
        var lpBungalow = new LoaiPhong { TenLoaiPhong = "Bungalow", SoNguoiToiDa = 4, MoTa = "Không gian riêng tư, gần gũi thiên nhiên." };

        db.LoaiPhong.AddRange(lpDon, lpDoi, lpVip, lpGiaDinh, lpBungalow);

        db.SaveChanges();

        // 4) Phòng (25 phòng đa dạng)
        var danhSachPhong = new List<Phong>();

        void ThemPhong(KhachSan ks, LoaiPhong lp, string soPhong, decimal gia, string huong, string? moTa = null, string? tienNghi = null, string? hinh = null, string? trangThai = null)
        {
            danhSachPhong.Add(new Phong
            {
                MaKhachSan = ks.MaKhachSan,
                MaLoaiPhong = lp.MaLoaiPhong,
                SoPhong = soPhong,
                GiaMotDem = gia,
                HuongPhong = huong,
                MoTa = moTa,
                TienNghi = tienNghi ?? "Wifi, Điều hòa, TV, Tủ lạnh",
                HinhAnh = hinh ?? "/images/phong/phong-mau.svg",
                TrangThai = trangThai ?? HangSo.TrangThaiPhongConTrong
            });
        }

        // Pullman (Bãi Sau)
        ThemPhong(ksPullman, lpVip, "P501", 3500000, HangSo.HuongViewBien, "Phòng VIP view biển, ban công rộng.", "Wifi, Điều hòa, TV, Tủ lạnh, Ban công hướng biển, Bồn tắm", "/images/phong/pullman-vip.svg");
        ThemPhong(ksPullman, lpDoi, "P302", 2200000, HangSo.HuongViewBien, "Phòng đôi view biển.", "Wifi, Điều hòa, TV, Ban công", "/images/phong/pullman-doi.svg");
        ThemPhong(ksPullman, lpGiaDinh, "P210", 2800000, HangSo.HuongViewThanhPho, "Phòng gia đình rộng rãi.", "Wifi, Điều hòa, TV, Tủ lạnh, Sofa", "/images/phong/pullman-gd.svg");
        ThemPhong(ksPullman, lpDon, "P105", 1500000, HangSo.HuongViewThanhPho);

        // Imperial (Bãi Trước)
        ThemPhong(ksImperial, lpVip, "I801", 3200000, HangSo.HuongViewBien, "VIP phong cách cổ điển.", "Wifi, Điều hòa, TV, Bồn tắm, Ban công", "/images/phong/imperial-vip.svg");
        ThemPhong(ksImperial, lpDoi, "I402", 2000000, HangSo.HuongViewThanhPho);
        ThemPhong(ksImperial, lpGiaDinh, "I510", 2600000, HangSo.HuongViewHoBoi, "Gia đình view hồ bơi.", "Wifi, Điều hòa, TV, Hồ bơi, Buffet sáng", "/images/phong/imperial-gd.svg");
        ThemPhong(ksImperial, lpDon, "I205", 1400000, HangSo.HuongViewThanhPho);

        // Malibu (Bãi Sau)
        ThemPhong(ksMalibu, lpDoi, "M301", 1800000, HangSo.HuongViewBien, "Phòng đôi, gần biển.", null, "/images/phong/malibu-doi.svg");
        ThemPhong(ksMalibu, lpVip, "M707", 3000000, HangSo.HuongViewBien, "VIP view biển tầng cao.", "Wifi, Điều hòa, TV, Ban công, Bồn tắm", "/images/phong/malibu-vip.svg");
        ThemPhong(ksMalibu, lpGiaDinh, "M410", 2400000, HangSo.HuongViewThanhPho);
        ThemPhong(ksMalibu, lpDon, "M118", 1200000, HangSo.HuongViewThanhPho);

        // Sammy (Bãi Trước)
        ThemPhong(ksSammy, lpDoi, "S206", 1600000, HangSo.HuongViewThanhPho);
        ThemPhong(ksSammy, lpGiaDinh, "S305", 2100000, HangSo.HuongViewHoBoi, "Gia đình view hồ bơi.", null, "/images/phong/sammy-gd.svg");
        ThemPhong(ksSammy, lpVip, "S606", 2500000, HangSo.HuongViewBien, "VIP view biển.", null, "/images/phong/sammy-vip.svg");
        ThemPhong(ksSammy, lpDon, "S102", 1100000, HangSo.HuongViewThanhPho);

        // Cap Saint Jacques (Bãi Dâu)
        ThemPhong(ksCapSaintJacques, lpDoi, "C201", 1300000, HangSo.HuongViewBien, "Phòng đôi view biển.", null, "/images/phong/csj-doi.svg");
        ThemPhong(ksCapSaintJacques, lpGiaDinh, "C305", 1700000, HangSo.HuongViewThanhPho);
        ThemPhong(ksCapSaintJacques, lpDon, "C101", 900000, HangSo.HuongViewThanhPho);
        ThemPhong(ksCapSaintJacques, lpVip, "C502", 2100000, HangSo.HuongViewBien, "VIP view biển.", null, "/images/phong/csj-vip.svg", HangSo.TrangThaiPhongBaoTri);

        // Lan Rừng (Bãi Dứa)
        ThemPhong(ksLanRung, lpBungalow, "LR-B01", 2900000, HangSo.HuongViewBien, "Bungalow riêng tư, gần biển.", "Wifi, Điều hòa, TV, Bồn tắm, Sân vườn", "/images/phong/lanrung-bungalow.svg");
        ThemPhong(ksLanRung, lpBungalow, "LR-B02", 2900000, HangSo.HuongViewBien, "Bungalow riêng tư.", "Wifi, Điều hòa, TV, Ban công", "/images/phong/lanrung-bungalow.svg");
        ThemPhong(ksLanRung, lpVip, "LR-701", 2700000, HangSo.HuongViewHoBoi, "VIP view hồ bơi.", null, "/images/phong/lanrung-vip.svg");
        ThemPhong(ksLanRung, lpDoi, "LR-302", 1700000, HangSo.HuongViewBien, "Phòng đôi view biển.", null, "/images/phong/lanrung-doi.svg");
        ThemPhong(ksLanRung, lpGiaDinh, "LR-405", 2200000, HangSo.HuongViewThanhPho);

        // Thêm vài phòng nữa để đủ ~25
        ThemPhong(ksPullman, lpDoi, "P303", 2100000, HangSo.HuongViewThanhPho);
        ThemPhong(ksMalibu, lpDoi, "M302", 1750000, HangSo.HuongViewBien);
        ThemPhong(ksImperial, lpDoi, "I403", 2050000, HangSo.HuongViewThanhPho);
        ThemPhong(ksSammy, lpDoi, "S207", 1550000, HangSo.HuongViewThanhPho);

        db.Phong.AddRange(danhSachPhong);
        db.SaveChanges();

        // 5) Đặt phòng mẫu (demo)
        var phongPullmanVip = db.Phong.AsNoTracking().First(p => p.SoPhong == "P501");
        var phongImperialDoi = db.Phong.AsNoTracking().First(p => p.SoPhong == "I402");
        var phongLanRungB01 = db.Phong.AsNoTracking().First(p => p.SoPhong == "LR-B01");

        var ngayHienTai = DateTime.Today;

        DatPhong TaoDatPhong(int maNguoiDung, int maPhong, DateTime nhan, DateTime tra, int soKhach, string trangThai)
        {
            var soDem = (tra - nhan).Days;
            if (soDem < 1) soDem = 1;

            var gia = db.Phong.AsNoTracking().Where(p => p.MaPhong == maPhong).Select(p => p.GiaMotDem).First();
            return new DatPhong
            {
                MaNguoiDung = maNguoiDung,
                MaPhong = maPhong,
                NgayNhanPhong = nhan,
                NgayTraPhong = tra,
                SoDem = soDem,
                SoKhach = soKhach,
                TongTien = soDem * gia,
                GhiChu = "Đặt phòng mẫu để demo.",
                PhuongThucThanhToan = HangSo.ThanhToanChuyenKhoan,
                TrangThai = trangThai,
                NgayDat = ngayHienTai.AddDays(-2)
            };
        }

        var dat1 = TaoDatPhong(khach1.MaNguoiDung, phongPullmanVip.MaPhong, ngayHienTai.AddDays(3), ngayHienTai.AddDays(5), 2, HangSo.TrangThaiDatChoXacNhan);
        var dat2 = TaoDatPhong(khach2.MaNguoiDung, phongImperialDoi.MaPhong, ngayHienTai.AddDays(1), ngayHienTai.AddDays(3), 2, HangSo.TrangThaiDatDaXacNhan);
        var dat3 = TaoDatPhong(khach1.MaNguoiDung, phongLanRungB01.MaPhong, ngayHienTai.AddDays(-10), ngayHienTai.AddDays(-7), 4, HangSo.TrangThaiDatDaTraPhong);

        db.DatPhong.AddRange(dat1, dat2, dat3);
        db.SaveChanges();
    }
}

