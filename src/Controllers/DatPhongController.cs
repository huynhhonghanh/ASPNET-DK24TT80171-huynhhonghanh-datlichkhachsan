using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using DatPhongKhachSanVungTau.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers;

public class DatPhongController : Controller
{
    private readonly AppDbContext _db;
    private readonly DatPhongService _datPhongService;

    public DatPhongController(AppDbContext db, DatPhongService datPhongService)
    {
        _db = db;
        _datPhongService = datPhongService;
    }

    private bool LayMaNguoiDung(out int maNguoiDung)
    {
        maNguoiDung = 0;
        var str = HttpContext.Session.GetString(PhienDangNhap.KhoaMaNguoiDung);
        return int.TryParse(str, out maNguoiDung);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [YeuCauDangNhap]
    public async Task<IActionResult> XacNhan([Bind(Prefix = "FormDatPhong")] DatPhongFormViewModel form)
    {
        if (!LayMaNguoiDung(out var maNguoiDung))
        {
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        if (!ModelState.IsValid)
        {
            TempData["Loi"] = "Vui lòng kiểm tra lại thông tin đặt phòng.";
            return RedirectToAction("ChiTiet", "Phong", new
            {
                id = form.MaPhong,
                ngayNhan = form.NgayNhanPhong.ToString("yyyy-MM-dd"),
                ngayTra = form.NgayTraPhong.ToString("yyyy-MM-dd"),
                soKhach = form.SoKhach
            });
        }

        var (thanhCong, loi, don) = await _datPhongService.TaoDatPhongAsync(maNguoiDung, form);

        if (!thanhCong || don == null)
        {
            TempData["Loi"] = loi ?? "Không thể đặt phòng. Vui lòng thử lại.";
            return RedirectToAction("ChiTiet", "Phong", new
            {
                id = form.MaPhong,
                ngayNhan = form.NgayNhanPhong.ToString("yyyy-MM-dd"),
                ngayTra = form.NgayTraPhong.ToString("yyyy-MM-dd"),
                soKhach = form.SoKhach
            });
        }

        TempData["ThongBao"] = "Đặt phòng thành công! Mã đặt phòng: #" + don.MaDatPhong;
        return RedirectToAction(nameof(ThanhCong), new { id = don.MaDatPhong });
    }

    [HttpGet]
    [YeuCauDangNhap]
    public async Task<IActionResult> ThanhCong(int id)
    {
        if (!LayMaNguoiDung(out var maNguoiDung))
        {
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        var don = await _db.DatPhong
            .AsNoTracking()
            .Include(d => d.Phong)
            .ThenInclude(p => p!.KhachSan)
            .FirstOrDefaultAsync(d => d.MaDatPhong == id && d.MaNguoiDung == maNguoiDung);

        if (don == null)
        {
            return NotFound();
        }

        var vm = new DatPhongThanhCongViewModel
        {
            MaDatPhong = don.MaDatPhong,
            TenKhachSan = don.Phong?.KhachSan?.TenKhachSan ?? "",
            SoPhong = don.Phong?.SoPhong ?? "",
            NgayNhanPhong = don.NgayNhanPhong,
            NgayTraPhong = don.NgayTraPhong,
            SoDem = don.SoDem,
            SoKhach = don.SoKhach,
            TongTien = don.TongTien,
            PhuongThucThanhToan = don.PhuongThucThanhToan,
            TrangThai = don.TrangThai,
            NgayDat = don.NgayDat
        };

        return View(vm);
    }

    /// <summary>
    /// Lịch sử đặt phòng của khách hàng đang đăng nhập.
    /// </summary>
    [HttpGet]
    [YeuCauDangNhap]
    public async Task<IActionResult> LichSu(string? trangThai)
    {
        if (!LayMaNguoiDung(out var maNguoiDung))
        {
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        var danhSach = await _datPhongService.LayLichSuDatPhongAsync(maNguoiDung, trangThai);

        var vm = new LichSuDatPhongViewModel
        {
            DanhSach = danhSach,
            LocTrangThai = trangThai
        };

        return View(vm);
    }

    /// <summary>
    /// Hủy đặt phòng (POST).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [YeuCauDangNhap]
    public async Task<IActionResult> Huy(int id)
    {
        if (!LayMaNguoiDung(out var maNguoiDung))
        {
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        var (thanhCong, loi) = await _datPhongService.HuyDatPhongAsync(id, maNguoiDung);

        if (thanhCong)
        {
            TempData["ThongBao"] = $"Đã hủy đặt phòng #{id} thành công.";
        }
        else
        {
            TempData["Loi"] = loi ?? "Không thể hủy đặt phòng.";
        }

        return RedirectToAction(nameof(LichSu));
    }
}
