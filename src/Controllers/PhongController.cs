using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using DatPhongKhachSanVungTau.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Controllers;

public class PhongController : Controller
{
    private readonly DatPhongService _datPhongService;
    private readonly AppDbContext _db;

    public PhongController(DatPhongService datPhongService, AppDbContext db)
    {
        _datPhongService = datPhongService;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> TimKiem(
        string? khuVuc,
        DateTime? ngayNhan,
        DateTime? ngayTra,
        int? soKhach,
        int? maLoaiPhong,
        int? xepHang,
        string? huongPhong,
        decimal? giaTu,
        decimal? giaDen,
        bool timKiem = false)
    {
        var vm = new TimKiemPhongViewModel
        {
            KhuVuc = khuVuc,
            NgayNhanPhong = ngayNhan ?? DateTime.Today.AddDays(1),
            NgayTraPhong = ngayTra ?? DateTime.Today.AddDays(2),
            SoKhach = soKhach is > 0 ? soKhach.Value : 2,
            MaLoaiPhong = maLoaiPhong,
            XepHang = xepHang,
            HuongPhong = huongPhong,
            GiaTu = giaTu,
            GiaDen = giaDen,
            DaTimKiem = timKiem
        };

        if (!vm.DaTimKiem && (ngayNhan.HasValue || ngayTra.HasValue || !string.IsNullOrEmpty(khuVuc)))
        {
            vm.DaTimKiem = true;
        }

        if (vm.DaTimKiem)
        {
            if (vm.NgayTraPhong <= vm.NgayNhanPhong)
            {
                TempData["Loi"] = "Ngày trả phòng phải sau ngày nhận phòng.";
                vm.NgayTraPhong = vm.NgayNhanPhong.AddDays(1);
            }

            if (vm.NgayNhanPhong.Date < DateTime.Today)
            {
                TempData["Loi"] = "Không thể chọn ngày nhận phòng trong quá khứ.";
                vm.NgayNhanPhong = DateTime.Today.AddDays(1);
                vm.NgayTraPhong = DateTime.Today.AddDays(2);
            }

            if (vm.GiaTu.HasValue && vm.GiaDen.HasValue && vm.GiaTu > vm.GiaDen)
            {
                (vm.GiaDen, vm.GiaTu) = (vm.GiaTu, vm.GiaDen);
            }
        }

        vm = await _datPhongService.TimKiemPhongAsync(vm);
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> ChiTiet(int id, DateTime? ngayNhan, DateTime? ngayTra, int? soKhach)
    {
        var phong = await _db.Phong
            .AsNoTracking()
            .Include(p => p.KhachSan)
            .Include(p => p.LoaiPhong)
            .FirstOrDefaultAsync(p => p.MaPhong == id);

        if (phong == null)
        {
            return NotFound();
        }

        var nhan = ngayNhan ?? DateTime.Today.AddDays(1);
        var tra = ngayTra ?? DateTime.Today.AddDays(2);
        var khach = soKhach is > 0 ? soKhach.Value : 2;

        var form = new DatPhongFormViewModel
        {
            MaPhong = id,
            NgayNhanPhong = nhan,
            NgayTraPhong = tra,
            SoKhach = khach,
            PhuongThucThanhToan = HangSo.ThanhToanTienMat
        };
        _datPhongService.CapNhatThongTinForm(form, phong);

        var conTrong = phong.TrangThai == HangSo.TrangThaiPhongConTrong
            && !_datPhongService.BiTrungLich(id, nhan, tra);

        var vm = new ChiTietPhongViewModel
        {
            Phong = phong,
            FormDatPhong = form,
            ConTrong = conTrong,
            DaDangNhap = !string.IsNullOrEmpty(HttpContext.Session.GetString(PhienDangNhap.KhoaMaNguoiDung))
        };

        return View(vm);
    }
}
