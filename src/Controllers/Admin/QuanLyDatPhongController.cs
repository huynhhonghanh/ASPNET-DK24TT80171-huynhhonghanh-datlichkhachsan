using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class QuanLyDatPhongController : Controller
{
    private readonly AppDbContext _db;

    public QuanLyDatPhongController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? trangThai)
    {
        var query = _db.DatPhong.AsNoTracking()
            .Include(d => d.NguoiDung)
            .Include(d => d.Phong)
            .ThenInclude(p => p!.KhachSan)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(trangThai))
        {
            query = query.Where(d => d.TrangThai == trangThai);
        }

        ViewBag.LocTrangThai = trangThai;
        var list = await query.OrderByDescending(d => d.NgayDat).ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> ChiTiet(int id)
    {
        var don = await _db.DatPhong.AsNoTracking()
            .Include(d => d.NguoiDung)
            .Include(d => d.Phong)
            .ThenInclude(p => p!.KhachSan)
            .Include(d => d.Phong)
            .ThenInclude(p => p!.LoaiPhong)
            .FirstOrDefaultAsync(d => d.MaDatPhong == id);

        if (don == null) return NotFound();
        return View(don);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CapNhatTrangThai(int id, string trangThai)
    {
        var don = await _db.DatPhong.FindAsync(id);
        if (don == null) return NotFound();

        var hopLe = new[]
        {
            HangSo.TrangThaiDatChoXacNhan,
            HangSo.TrangThaiDatDaXacNhan,
            HangSo.TrangThaiDatDaNhanPhong,
            HangSo.TrangThaiDatDaTraPhong,
            HangSo.TrangThaiDatDaHuy
        };

        if (!hopLe.Contains(trangThai))
        {
            TempData["Loi"] = "Trạng thái không hợp lệ.";
            return RedirectToAction(nameof(ChiTiet), new { id });
        }

        don.TrangThai = trangThai;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = $"Đã cập nhật trạng thái đặt phòng #{id} thành \"{trangThai}\".";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> XacNhan(int id)
    {
        return await CapNhatTrangThai(id, HangSo.TrangThaiDatDaXacNhan);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Huy(int id)
    {
        return await CapNhatTrangThai(id, HangSo.TrangThaiDatDaHuy);
    }
}
