using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Khách sạn ven biển Vũng Tàu.
/// </summary>
[Table("KhachSan")]
public class KhachSan
{
    [Key]
    [Display(Name = "Mã khách sạn")]
    public int MaKhachSan { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách sạn")]
    [StringLength(150)]
    [Display(Name = "Tên khách sạn")]
    public string TenKhachSan { get; set; } = string.Empty;

    /// <summary>Bãi Trước, Bãi Sau, Bãi Dâu, Bãi Dứa, Bãi Vọng Nguyệt</summary>
    [Required(ErrorMessage = "Vui lòng chọn khu vực bãi biển")]
    [StringLength(50)]
    [Display(Name = "Khu vực bãi biển")]
    public string KhuVuc { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    [StringLength(255)]
    [Display(Name = "Địa chỉ chi tiết")]
    public string DiaChiChiTiet { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [StringLength(500)]
    [Display(Name = "Hình ảnh")]
    public string? HinhAnh { get; set; }

    [Range(1, 5, ErrorMessage = "Xếp hạng từ 1 đến 5 sao")]
    [Display(Name = "Xếp hạng (sao)")]
    public int XepHang { get; set; }

    [Display(Name = "Khoảng cách đến biển (m)")]
    public int KhoangCachBien { get; set; }

    public ICollection<Phong> DanhSachPhong { get; set; } = new List<Phong>();
}
