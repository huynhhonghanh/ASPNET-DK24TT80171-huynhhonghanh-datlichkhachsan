using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class QuanLyLoaiPhongController : Controller
{
    private readonly AppDbContext _db;

    public QuanLyLoaiPhongController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var list = await _db.LoaiPhong.AsNoTracking().OrderBy(l => l.TenLoaiPhong).ToListAsync();
        return View(list);
    }

    public IActionResult Tao() => View(new LoaiPhong { SoNguoiToiDa = 2 });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Tao(LoaiPhong model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.LoaiPhong.Add(model);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã thêm loại phòng.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Sua(int id)
    {
        var lp = await _db.LoaiPhong.FindAsync(id);
        if (lp == null) return NotFound();
        return View(lp);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sua(int id, LoaiPhong model)
    {
        if (id != model.MaLoaiPhong) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var lp = await _db.LoaiPhong.FindAsync(id);
        if (lp == null) return NotFound();

        lp.TenLoaiPhong = model.TenLoaiPhong;
        lp.SoNguoiToiDa = model.SoNguoiToiDa;
        lp.MoTa = model.MoTa;
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã cập nhật loại phòng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id)
    {
        var lp = await _db.LoaiPhong.Include(l => l.DanhSachPhong).FirstOrDefaultAsync(l => l.MaLoaiPhong == id);
        if (lp == null) return NotFound();

        if (lp.DanhSachPhong.Any())
        {
            TempData["Loi"] = "Không thể xóa loại phòng đang được sử dụng.";
            return RedirectToAction(nameof(Index));
        }

        _db.LoaiPhong.Remove(lp);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã xóa loại phòng.";
        return RedirectToAction(nameof(Index));
    }
}
