using System.Diagnostics;
using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, AppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var khachSanNoiBat = await _db.KhachSan
            .AsNoTracking()
            .Select(k => new KhachSanNoiBatViewModel
            {
                MaKhachSan = k.MaKhachSan,
                TenKhachSan = k.TenKhachSan,
                KhuVuc = k.KhuVuc,
                HinhAnh = k.HinhAnh,
                XepHang = k.XepHang,
                KhoangCachBien = k.KhoangCachBien,
                GiaTu = k.DanhSachPhong
                    .Where(p => p.TrangThai != HangSo.TrangThaiPhongBaoTri)
                    .Min(p => (decimal?)p.GiaMotDem) ?? 0,
                SoPhong = k.DanhSachPhong.Count(p => p.TrangThai == HangSo.TrangThaiPhongConTrong)
            })
            .OrderByDescending(k => k.XepHang)
            .ThenBy(k => k.GiaTu)
            .Take(6)
            .ToListAsync();

        var vm = new TrangChuViewModel
        {
            TimKiem = new TimKiemNhanhViewModel
            {
                NgayNhanPhong = DateTime.Today.AddDays(1),
                NgayTraPhong = DateTime.Today.AddDays(2),
                SoKhach = 2
            },
            KhachSanNoiBat = khachSanNoiBat
        };

        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
