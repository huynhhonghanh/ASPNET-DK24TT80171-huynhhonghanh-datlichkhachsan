using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class QuanLyKhachHangController : Controller
{
    private readonly AppDbContext _db;

    public QuanLyKhachHangController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var list = await _db.NguoiDung.AsNoTracking()
            .Where(n => n.VaiTro == HangSo.VaiTroKhachHang)
            .OrderByDescending(n => n.MaNguoiDung)
            .ToListAsync();
        return View(list);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> KhoaTaiKhoan(int id)
    {
        var nd = await _db.NguoiDung.FindAsync(id);
        if (nd == null || nd.VaiTro != HangSo.VaiTroKhachHang) return NotFound();

        nd.TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanBiKhoa;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã khóa tài khoản {nd.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoKhoaTaiKhoan(int id)
    {
        var nd = await _db.NguoiDung.FindAsync(id);
        if (nd == null || nd.VaiTro != HangSo.VaiTroKhachHang) return NotFound();

        nd.TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanHoatDong;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã mở khóa tài khoản {nd.TenDangNhap}.";
        return RedirectToAction(nameof(Index));
    }
}
