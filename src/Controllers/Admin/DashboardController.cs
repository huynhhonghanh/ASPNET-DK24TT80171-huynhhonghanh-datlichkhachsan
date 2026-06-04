using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class DashboardController : Controller
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.TongKhachHang = await _db.NguoiDung.CountAsync(n => n.VaiTro == HangSo.VaiTroKhachHang);
        ViewBag.TongKhachSan = await _db.KhachSan.CountAsync();
        ViewBag.TongPhong = await _db.Phong.CountAsync();
        ViewBag.TongDatPhong = await _db.DatPhong.CountAsync();
        ViewBag.ChoXacNhan = await _db.DatPhong.CountAsync(d => d.TrangThai == HangSo.TrangThaiDatChoXacNhan);
        ViewBag.DoanhThuThang = await _db.DatPhong
            .Where(d => d.TrangThai != HangSo.TrangThaiDatDaHuy
                && d.NgayDat.Month == DateTime.Now.Month
                && d.NgayDat.Year == DateTime.Now.Year)
            .SumAsync(d => d.TongTien);
        return View();
    }
}
