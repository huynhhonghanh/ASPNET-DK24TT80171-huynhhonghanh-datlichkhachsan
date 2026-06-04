using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class QuanLyKhachSanController : Controller
{
    private readonly AppDbContext _db;
    private readonly AnhUploadService _anhUpload;

    public QuanLyKhachSanController(AppDbContext db, AnhUploadService anhUpload)
    {
        _db = db;
        _anhUpload = anhUpload;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _db.KhachSan.AsNoTracking().OrderBy(k => k.TenKhachSan).ToListAsync();
        return View(list);
    }

    public IActionResult Tao()
    {
        ViewBag.DanhSachKhuVuc = HangSo.DanhSachKhuVuc;
        return View(new KhachSan { XepHang = 3, KhoangCachBien = 100 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Tao(KhachSan model, IFormFile? hinhAnh)
    {
        ViewBag.DanhSachKhuVuc = HangSo.DanhSachKhuVuc;
        if (!ModelState.IsValid) return View(model);

        var duongDan = await _anhUpload.LuuAnhAsync(hinhAnh, "khachsan");
        if (duongDan != null) model.HinhAnh = duongDan;

        _db.KhachSan.Add(model);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã thêm khách sạn thành công.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Sua(int id)
    {
        var ks = await _db.KhachSan.FindAsync(id);
        if (ks == null) return NotFound();
        ViewBag.DanhSachKhuVuc = HangSo.DanhSachKhuVuc;
        return View(ks);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sua(int id, KhachSan model, IFormFile? hinhAnh)
    {
        ViewBag.DanhSachKhuVuc = HangSo.DanhSachKhuVuc;
        if (id != model.MaKhachSan) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var ks = await _db.KhachSan.FindAsync(id);
        if (ks == null) return NotFound();

        ks.TenKhachSan = model.TenKhachSan;
        ks.KhuVuc = model.KhuVuc;
        ks.DiaChiChiTiet = model.DiaChiChiTiet;
        ks.MoTa = model.MoTa;
        ks.XepHang = model.XepHang;
        ks.KhoangCachBien = model.KhoangCachBien;

        var duongDan = await _anhUpload.LuuAnhAsync(hinhAnh, "khachsan");
        if (duongDan != null)
        {
            _anhUpload.XoaAnhCu(ks.HinhAnh);
            ks.HinhAnh = duongDan;
        }

        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã cập nhật khách sạn.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id)
    {
        var ks = await _db.KhachSan.Include(k => k.DanhSachPhong).FirstOrDefaultAsync(k => k.MaKhachSan == id);
        if (ks == null) return NotFound();

        if (ks.DanhSachPhong.Any())
        {
            TempData["Loi"] = "Không thể xóa khách sạn đang có phòng. Hãy xóa phòng trước.";
            return RedirectToAction(nameof(Index));
        }

        _anhUpload.XoaAnhCu(ks.HinhAnh);
        _db.KhachSan.Remove(ks);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã xóa khách sạn.";
        return RedirectToAction(nameof(Index));
    }
}
