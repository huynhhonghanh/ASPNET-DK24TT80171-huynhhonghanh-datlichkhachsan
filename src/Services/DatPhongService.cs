using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Services;

/// <summary>
/// Logic kiểm tra phòng trống và tính tiền đặt phòng.
/// </summary>
public class DatPhongService
{
    private readonly AppDbContext _db;

    public DatPhongService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Số đêm lưu trú.
    /// </summary>
    public int TinhSoDem(DateTime ngayNhan, DateTime ngayTra)
    {
        var soDem = (ngayTra.Date - ngayNhan.Date).Days;
        return soDem < 1 ? 1 : soDem;
    }

    /// <summary>
    /// Tổng tiền = số đêm × giá một đêm.
    /// </summary>
    public decimal TinhTongTien(decimal giaMotDem, DateTime ngayNhan, DateTime ngayTra)
    {
        return TinhSoDem(ngayNhan, ngayTra) * giaMotDem;
    }

    /// <summary>
    /// Kiểm tra phòng có bị trùng lịch đặt trong khoảng ngày không.
    /// </summary>
    public bool BiTrungLich(int maPhong, DateTime ngayNhan, DateTime ngayTra)
    {
        return _db.DatPhong.Any(d =>
            d.MaPhong == maPhong
            && d.TrangThai != HangSo.TrangThaiDatDaHuy
            && d.NgayNhanPhong.Date < ngayTra.Date
            && d.NgayTraPhong.Date > ngayNhan.Date);
    }

    /// <summary>
    /// Truy vấn phòng còn trống theo khoảng ngày (chưa tính bộ lọc khác).
    /// </summary>
    public IQueryable<Phong> QueryPhongTrong(DateTime ngayNhan, DateTime ngayTra)
    {
        var nhan = ngayNhan.Date;
        var tra = ngayTra.Date;

        return _db.Phong
            .AsNoTracking()
            .Include(p => p.KhachSan)
            .Include(p => p.LoaiPhong)
            .Where(p => p.TrangThai == HangSo.TrangThaiPhongConTrong)
            .Where(p => !_db.DatPhong.Any(d =>
                d.MaPhong == p.MaPhong
                && d.TrangThai != HangSo.TrangThaiDatDaHuy
                && d.NgayNhanPhong.Date < tra
                && d.NgayTraPhong.Date > nhan));
    }

    /// <summary>
    /// Tìm kiếm phòng với đầy đủ bộ lọc.
    /// </summary>
    public async Task<TimKiemPhongViewModel> TimKiemPhongAsync(TimKiemPhongViewModel boLoc)
    {
        var giaStats = await _db.Phong
            .Where(p => p.TrangThai != HangSo.TrangThaiPhongBaoTri)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                Min = g.Min(p => p.GiaMotDem),
                Max = g.Max(p => p.GiaMotDem)
            })
            .FirstOrDefaultAsync();

        boLoc.GiaMinHeThong = giaStats?.Min ?? 500_000;
        boLoc.GiaMaxHeThong = giaStats?.Max ?? 5_000_000;
        boLoc.DanhSachLoaiPhong = await _db.LoaiPhong.AsNoTracking().OrderBy(l => l.TenLoaiPhong).ToListAsync();

        if (!boLoc.DaTimKiem)
        {
            boLoc.KetQua = new List<PhongTimKiemItemViewModel>();
            return boLoc;
        }

        var query = QueryPhongTrong(boLoc.NgayNhanPhong, boLoc.NgayTraPhong);

        // Lọc số khách (sức chứa loại phòng)
        query = query.Where(p => p.LoaiPhong != null && p.LoaiPhong.SoNguoiToiDa >= boLoc.SoKhach);

        if (!string.IsNullOrWhiteSpace(boLoc.KhuVuc))
        {
            query = query.Where(p => p.KhachSan != null && p.KhachSan.KhuVuc == boLoc.KhuVuc);
        }

        if (boLoc.MaLoaiPhong.HasValue && boLoc.MaLoaiPhong > 0)
        {
            query = query.Where(p => p.MaLoaiPhong == boLoc.MaLoaiPhong);
        }

        if (boLoc.XepHang.HasValue && boLoc.XepHang > 0)
        {
            query = query.Where(p => p.KhachSan != null && p.KhachSan.XepHang == boLoc.XepHang);
        }

        if (!string.IsNullOrWhiteSpace(boLoc.HuongPhong))
        {
            query = query.Where(p => p.HuongPhong == boLoc.HuongPhong);
        }

        if (boLoc.GiaTu.HasValue && boLoc.GiaTu > 0)
        {
            query = query.Where(p => p.GiaMotDem >= boLoc.GiaTu);
        }

        if (boLoc.GiaDen.HasValue && boLoc.GiaDen > 0)
        {
            query = query.Where(p => p.GiaMotDem <= boLoc.GiaDen);
        }

        var soDem = TinhSoDem(boLoc.NgayNhanPhong, boLoc.NgayTraPhong);

        var danhSach = await query
            .OrderBy(p => p.GiaMotDem)
            .ThenByDescending(p => p.KhachSan!.XepHang)
            .Select(p => new PhongTimKiemItemViewModel
            {
                MaPhong = p.MaPhong,
                SoPhong = p.SoPhong,
                TenKhachSan = p.KhachSan!.TenKhachSan,
                KhuVuc = p.KhachSan.KhuVuc,
                XepHang = p.KhachSan.XepHang,
                KhoangCachBien = p.KhachSan.KhoangCachBien,
                TenLoaiPhong = p.LoaiPhong!.TenLoaiPhong,
                SoNguoiToiDa = p.LoaiPhong.SoNguoiToiDa,
                HuongPhong = p.HuongPhong,
                HinhAnh = p.HinhAnh,
                TienNghi = p.TienNghi,
                GiaMotDem = p.GiaMotDem,
                SoDem = soDem,
                TongTien = soDem * p.GiaMotDem
            })
            .ToListAsync();

        boLoc.KetQua = danhSach;
        return boLoc;
    }

    /// <summary>
    /// Validate ngày và số khách — trả về thông báo lỗi (null nếu hợp lệ).
    /// </summary>
    public string? KiemTraNgayVaKhach(DateTime ngayNhan, DateTime ngayTra, int soKhach, int soNguoiToiDaPhong)
    {
        if (ngayNhan.Date < DateTime.Today)
        {
            return "Không thể chọn ngày nhận phòng trong quá khứ.";
        }

        if (ngayTra.Date <= ngayNhan.Date)
        {
            return "Ngày trả phòng phải sau ngày nhận phòng.";
        }

        if (soKhach < 1 || soKhach > 20)
        {
            return "Số khách từ 1 đến 20.";
        }

        if (soKhach > soNguoiToiDaPhong)
        {
            return $"Số khách vượt sức chứa phòng (tối đa {soNguoiToiDaPhong} người).";
        }

        return null;
    }

    /// <summary>
    /// Tạo đơn đặt phòng mới.
    /// </summary>
    public async Task<(bool ThanhCong, string? ThongBaoLoi, Models.DatPhong? DonDat)> TaoDatPhongAsync(
        int maNguoiDung, DatPhongFormViewModel form)
    {
        var phong = await _db.Phong
            .Include(p => p.LoaiPhong)
            .Include(p => p.KhachSan)
            .FirstOrDefaultAsync(p => p.MaPhong == form.MaPhong);

        if (phong == null)
        {
            return (false, "Phòng không tồn tại.", null);
        }

        if (phong.TrangThai != HangSo.TrangThaiPhongConTrong)
        {
            return (false, "Phòng không ở trạng thái còn trống.", null);
        }

        var soNguoiToiDa = phong.LoaiPhong?.SoNguoiToiDa ?? 1;
        var loi = KiemTraNgayVaKhach(form.NgayNhanPhong, form.NgayTraPhong, form.SoKhach, soNguoiToiDa);
        if (loi != null)
        {
            return (false, loi, null);
        }

        if (BiTrungLich(form.MaPhong, form.NgayNhanPhong, form.NgayTraPhong))
        {
            return (false, "Phòng đã có đặt phòng trùng lịch trong khoảng thời gian này. Vui lòng chọn ngày khác.", null);
        }

        var soDem = TinhSoDem(form.NgayNhanPhong, form.NgayTraPhong);
        var tongTien = TinhTongTien(phong.GiaMotDem, form.NgayNhanPhong, form.NgayTraPhong);

        var don = new Models.DatPhong
        {
            MaNguoiDung = maNguoiDung,
            MaPhong = form.MaPhong,
            NgayNhanPhong = form.NgayNhanPhong.Date,
            NgayTraPhong = form.NgayTraPhong.Date,
            SoDem = soDem,
            SoKhach = form.SoKhach,
            TongTien = tongTien,
            GhiChu = form.GhiChu?.Trim(),
            PhuongThucThanhToan = form.PhuongThucThanhToan,
            TrangThai = HangSo.TrangThaiDatChoXacNhan,
            NgayDat = DateTime.Now
        };

        _db.DatPhong.Add(don);
        await _db.SaveChangesAsync();

        return (true, null, don);
    }

    /// <summary>
    /// Điền thông tin hiển thị cho form đặt phòng từ entity Phong.
    /// </summary>
    public void CapNhatThongTinForm(DatPhongFormViewModel form, Phong phong)
    {
        form.TenKhachSan = phong.KhachSan?.TenKhachSan;
        form.SoPhong = phong.SoPhong;
        form.TenLoaiPhong = phong.LoaiPhong?.TenLoaiPhong;
        form.SoNguoiToiDa = phong.LoaiPhong?.SoNguoiToiDa ?? 1;
        form.GiaMotDem = phong.GiaMotDem;
        form.SoDem = TinhSoDem(form.NgayNhanPhong, form.NgayTraPhong);
        form.TongTien = TinhTongTien(phong.GiaMotDem, form.NgayNhanPhong, form.NgayTraPhong);
        form.HinhAnh = phong.HinhAnh;
    }

    /// <summary>
    /// Lấy lịch sử đặt phòng của khách hàng.
    /// </summary>
    public async Task<List<DatPhongLichSuItemViewModel>> LayLichSuDatPhongAsync(int maNguoiDung, string? locTrangThai = null)
    {
        var query = _db.DatPhong
            .AsNoTracking()
            .Include(d => d.Phong)
            .ThenInclude(p => p!.KhachSan)
            .Where(d => d.MaNguoiDung == maNguoiDung);

        if (!string.IsNullOrWhiteSpace(locTrangThai))
        {
            query = query.Where(d => d.TrangThai == locTrangThai);
        }

        var danhSach = await query
            .OrderByDescending(d => d.NgayDat)
            .Select(d => new DatPhongLichSuItemViewModel
            {
                MaDatPhong = d.MaDatPhong,
                TenKhachSan = d.Phong!.KhachSan!.TenKhachSan,
                KhuVuc = d.Phong.KhachSan.KhuVuc,
                SoPhong = d.Phong.SoPhong,
                HinhAnh = d.Phong.HinhAnh,
                NgayNhanPhong = d.NgayNhanPhong,
                NgayTraPhong = d.NgayTraPhong,
                SoDem = d.SoDem,
                SoKhach = d.SoKhach,
                TongTien = d.TongTien,
                PhuongThucThanhToan = d.PhuongThucThanhToan,
                TrangThai = d.TrangThai,
                NgayDat = d.NgayDat,
                CoTheHuy = d.TrangThai == HangSo.TrangThaiDatChoXacNhan
                    || d.TrangThai == HangSo.TrangThaiDatDaXacNhan
            })
            .ToListAsync();

        return danhSach;
    }

    /// <summary>
    /// Hủy đặt phòng (chỉ Chờ xác nhận / Đã xác nhận).
    /// </summary>
    public async Task<(bool ThanhCong, string? ThongBaoLoi)> HuyDatPhongAsync(int maDatPhong, int maNguoiDung)
    {
        var don = await _db.DatPhong
            .FirstOrDefaultAsync(d => d.MaDatPhong == maDatPhong && d.MaNguoiDung == maNguoiDung);

        if (don == null)
        {
            return (false, "Không tìm thấy đơn đặt phòng.");
        }

        if (!HangSo.CoTheHuyDatPhong(don.TrangThai))
        {
            return (false, "Không thể hủy đặt phòng ở trạng thái hiện tại (chỉ hủy khi Chờ xác nhận hoặc Đã xác nhận).");
        }

        don.TrangThai = HangSo.TrangThaiDatDaHuy;
        await _db.SaveChangesAsync();

        return (true, null);
    }
}
