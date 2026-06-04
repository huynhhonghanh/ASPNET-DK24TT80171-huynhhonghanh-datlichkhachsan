using DatPhongKhachSanVungTau.Filters;
using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers.Admin;

[YeuCauVaiTro("Admin")]
public class QuanLyPhongController : Controller
{
    private readonly AppDbContext _db;
    private readonly AnhUploadService _anhUpload;

    public QuanLyPhongController(AppDbContext db, AnhUploadService anhUpload)
    {
        _db = db;
        _anhUpload = anhUpload;
    }

    private async Task NapDropdownAsync()
    {
        ViewBag.KhachSans = new SelectList(
            await _db.KhachSan.OrderBy(k => k.TenKhachSan).ToListAsync(),
            "MaKhachSan", "TenKhachSan");
        ViewBag.LoaiPhongs = new SelectList(
            await _db.LoaiPhong.OrderBy(l => l.TenLoaiPhong).ToListAsync(),
            "MaLoaiPhong", "TenLoaiPhong");
        ViewBag.TrangThais = new SelectList(new[]
        {
            HangSo.TrangThaiPhongConTrong,
            HangSo.TrangThaiPhongDangSuDung,
            HangSo.TrangThaiPhongBaoTri
        });
        ViewBag.HuongPhongs = new SelectList(new[]
        {
            HangSo.HuongViewBien,
            HangSo.HuongViewThanhPho,
            HangSo.HuongViewHoBoi
        });
    }

    public async Task<IActionResult> Index(int? maKhachSan)
    {
        var query = _db.Phong.AsNoTracking()
            .Include(p => p.KhachSan)
            .Include(p => p.LoaiPhong)
            .AsQueryable();

        if (maKhachSan.HasValue && maKhachSan > 0)
        {
            query = query.Where(p => p.MaKhachSan == maKhachSan);
        }

        ViewBag.KhachSans = new SelectList(
            await _db.KhachSan.OrderBy(k => k.TenKhachSan).ToListAsync(),
            "MaKhachSan", "TenKhachSan", maKhachSan);

        var list = await query.OrderBy(p => p.KhachSan!.TenKhachSan).ThenBy(p => p.SoPhong).ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> Tao()
    {
        await NapDropdownAsync();
        return View(new Phong
        {
            TrangThai = HangSo.TrangThaiPhongConTrong,
            HuongPhong = HangSo.HuongViewBien,
            GiaMotDem = 1_000_000
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Tao(Phong model, IFormFile? hinhAnh)
    {
        await NapDropdownAsync();
        if (!ModelState.IsValid) return View(model);

        var trung = await _db.Phong.AnyAsync(p => p.MaKhachSan == model.MaKhachSan && p.SoPhong == model.SoPhong);
        if (trung)
        {
            ModelState.AddModelError(nameof(model.SoPhong), "Số phòng đã tồn tại trong khách sạn này.");
            return View(model);
        }

        var duongDan = await _anhUpload.LuuAnhAsync(hinhAnh, "phong");
        if (duongDan != null) model.HinhAnh = duongDan;

        _db.Phong.Add(model);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã thêm phòng.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Sua(int id)
    {
        var phong = await _db.Phong.FindAsync(id);
        if (phong == null) return NotFound();
        await NapDropdownAsync();
        return View(phong);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sua(int id, Phong model, IFormFile? hinhAnh)
    {
        await NapDropdownAsync();
        if (id != model.MaPhong) return NotFound();
        if (!ModelState.IsValid) return View(model);

        var phong = await _db.Phong.FindAsync(id);
        if (phong == null) return NotFound();

        var trung = await _db.Phong.AnyAsync(p =>
            p.MaKhachSan == model.MaKhachSan && p.SoPhong == model.SoPhong && p.MaPhong != id);
        if (trung)
        {
            ModelState.AddModelError(nameof(model.SoPhong), "Số phòng đã tồn tại trong khách sạn này.");
            return View(model);
        }

        phong.MaKhachSan = model.MaKhachSan;
        phong.MaLoaiPhong = model.MaLoaiPhong;
        phong.SoPhong = model.SoPhong;
        phong.GiaMotDem = model.GiaMotDem;
        phong.HuongPhong = model.HuongPhong;
        phong.MoTa = model.MoTa;
        phong.TienNghi = model.TienNghi;
        phong.TrangThai = model.TrangThai;

        var duongDan = await _anhUpload.LuuAnhAsync(hinhAnh, "phong");
        if (duongDan != null)
        {
            _anhUpload.XoaAnhCu(phong.HinhAnh);
            phong.HinhAnh = duongDan;
        }

        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã cập nhật phòng.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Xoa(int id)
    {
        var phong = await _db.Phong.Include(p => p.DanhSachDatPhong).FirstOrDefaultAsync(p => p.MaPhong == id);
        if (phong == null) return NotFound();

        if (phong.DanhSachDatPhong.Any(d => d.TrangThai != HangSo.TrangThaiDatDaHuy))
        {
            TempData["Loi"] = "Không thể xóa phòng đang có đặt phòng.";
            return RedirectToAction(nameof(Index));
        }

        _anhUpload.XoaAnhCu(phong.HinhAnh);
        _db.Phong.Remove(phong);
        await _db.SaveChangesAsync();
        TempData["ThongBao"] = "Đã xóa phòng.";
        return RedirectToAction(nameof(Index));
    }
}
