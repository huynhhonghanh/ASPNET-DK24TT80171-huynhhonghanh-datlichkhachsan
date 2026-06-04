using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Loại phòng (đơn, đôi, VIP, gia đình, bungalow...).
/// </summary>
[Table("LoaiPhong")]
public class LoaiPhong
{
    [Key]
    [Display(Name = "Mã loại phòng")]
    public int MaLoaiPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên loại phòng")]
    [StringLength(80)]
    [Display(Name = "Tên loại phòng")]
    public string TenLoaiPhong { get; set; } = string.Empty;

    [Range(1, 20, ErrorMessage = "Số người tối đa từ 1 đến 20")]
    [Display(Name = "Số người tối đa")]
    public int SoNguoiToiDa { get; set; }

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    public ICollection<Phong> DanhSachPhong { get; set; } = new List<Phong>();
}
