using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Người dùng hệ thống (Admin hoặc Khách hàng).
/// </summary>
[Table("NguoiDung")]
public class NguoiDung
{
    [Key]
    [Display(Name = "Mã người dùng")]
    public int MaNguoiDung { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [StringLength(50)]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(100)]
    [Display(Name = "Họ tên")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(100)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [StringLength(15)]
    [Display(Name = "Số điện thoại")]
    public string? SoDienThoai { get; set; }

    [StringLength(12)]
    [Display(Name = "CCCD")]
    public string? CCCD { get; set; }

    [StringLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }

    /// <summary>Admin hoặc KhachHang</summary>
    [Required]
    [StringLength(20)]
    [Display(Name = "Vai trò")]
    public string VaiTro { get; set; } = HangSo.VaiTroKhachHang;

    /// <summary>HoatDong hoặc BiKhoa (dùng ở Bước 10)</summary>
    [StringLength(20)]
    [Display(Name = "Trạng thái tài khoản")]
    public string TrangThaiTaiKhoan { get; set; } = HangSo.TrangThaiTaiKhoanHoatDong;

    public ICollection<DatPhong> DanhSachDatPhong { get; set; } = new List<DatPhong>();
}
