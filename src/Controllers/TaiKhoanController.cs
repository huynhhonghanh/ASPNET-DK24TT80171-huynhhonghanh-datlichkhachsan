using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using DatPhongKhachSanVungTau.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers;

public class TaiKhoanController : Controller
{
    private readonly AppDbContext _db;

    public TaiKhoanController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult DangNhap(string? returnUrl = null)
    {
        var vm = new DangNhapViewModel { ReturnUrl = returnUrl };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DangNhap(DangNhapViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var nguoiDung = await _db.NguoiDung
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.TenDangNhap == vm.TenDangNhap);

        if (nguoiDung == null)
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(vm);
        }

        if (string.Equals(nguoiDung.TrangThaiTaiKhoan, HangSo.TrangThaiTaiKhoanBiKhoa, StringComparison.OrdinalIgnoreCase))
        {
            ModelState.AddModelError(string.Empty, "Tài khoản đang bị khóa. Vui lòng liên hệ quản trị viên.");
            return View(vm);
        }

        var hopLe = BCrypt.Net.BCrypt.Verify(vm.MatKhau, nguoiDung.MatKhau);
        if (!hopLe)
        {
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(vm);
        }

        HttpContext.Session.SetString(PhienDangNhap.KhoaMaNguoiDung, nguoiDung.MaNguoiDung.ToString());
        HttpContext.Session.SetString(PhienDangNhap.KhoaTenDangNhap, nguoiDung.TenDangNhap);
        HttpContext.Session.SetString(PhienDangNhap.KhoaHoTen, nguoiDung.HoTen);
        HttpContext.Session.SetString(PhienDangNhap.KhoaVaiTro, nguoiDung.VaiTro);

        if (!string.IsNullOrWhiteSpace(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
        {
            return Redirect(vm.ReturnUrl);
        }

        if (string.Equals(nguoiDung.VaiTro, HangSo.VaiTroAdmin, StringComparison.OrdinalIgnoreCase))
        {
            return Redirect("/Admin/Dashboard");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult DangKy()
    {
        return View(new DangKyViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DangKy(DangKyViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var tenDaTonTai = await _db.NguoiDung.AnyAsync(n => n.TenDangNhap == vm.TenDangNhap);
        if (tenDaTonTai)
        {
            ModelState.AddModelError(nameof(vm.TenDangNhap), "Tên đăng nhập đã tồn tại.");
            return View(vm);
        }

        var emailDaTonTai = await _db.NguoiDung.AnyAsync(n => n.Email == vm.Email);
        if (emailDaTonTai)
        {
            ModelState.AddModelError(nameof(vm.Email), "Email đã được sử dụng.");
            return View(vm);
        }

        var nguoiDung = new NguoiDung
        {
            TenDangNhap = vm.TenDangNhap.Trim(),
            MatKhau = BCrypt.Net.BCrypt.HashPassword(vm.MatKhau),
            HoTen = vm.HoTen.Trim(),
            Email = vm.Email.Trim(),
            SoDienThoai = vm.SoDienThoai?.Trim(),
            CCCD = vm.CCCD?.Trim(),
            DiaChi = vm.DiaChi?.Trim(),
            VaiTro = HangSo.VaiTroKhachHang,
            TrangThaiTaiKhoan = HangSo.TrangThaiTaiKhoanHoatDong
        };

        _db.NguoiDung.Add(nguoiDung);
        await _db.SaveChangesAsync();

        TempData["ThongBao"] = "Đăng ký thành công. Vui lòng đăng nhập.";
        return RedirectToAction(nameof(DangNhap));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DangXuat()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult QuenMatKhau()
    {
        return View(new QuenMatKhauViewModel());
    }

    /// <summary>
    /// Demo đồ án: reset mật khẩu và hiển thị mật khẩu tạm ngay trên giao diện (không gửi email thật).
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuenMatKhau(QuenMatKhauViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var nguoiDung = await _db.NguoiDung.FirstOrDefaultAsync(n => n.Email == vm.Email);
        if (nguoiDung == null)
        {
            ModelState.AddModelError(nameof(vm.Email), "Không tìm thấy tài khoản theo email này.");
            return View(vm);
        }

        var matKhauTam = TaoMatKhauTam();
        nguoiDung.MatKhau = BCrypt.Net.BCrypt.HashPassword(matKhauTam);
        await _db.SaveChangesAsync();

        ViewBag.MatKhauTam = matKhauTam;
        return View(vm);
    }

    private static string TaoMatKhauTam()
    {
        // Mật khẩu tạm đơn giản để demo
        var rnd = Random.Shared.Next(100000, 999999);
        return $"VT{rnd}";
    }
}

