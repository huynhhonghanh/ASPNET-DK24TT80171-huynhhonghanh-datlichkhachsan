using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Phòng thuộc khách sạn.
/// </summary>
[Table("Phong")]
public class Phong
{
    [Key]
    [Display(Name = "Mã phòng")]
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số phòng")]
    [StringLength(20)]
    [Display(Name = "Số phòng")]
    public string SoPhong { get; set; } = string.Empty;

    [Display(Name = "Khách sạn")]
    public int MaKhachSan { get; set; }

    [ForeignKey(nameof(MaKhachSan))]
    public KhachSan? KhachSan { get; set; }

    [Display(Name = "Loại phòng")]
    public int MaLoaiPhong { get; set; }

    [ForeignKey(nameof(MaLoaiPhong))]
    public LoaiPhong? LoaiPhong { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,0)")]
    [Display(Name = "Giá một đêm (VNĐ)")]
    public decimal GiaMotDem { get; set; }

    [StringLength(50)]
    [Display(Name = "Hướng phòng")]
    public string HuongPhong { get; set; } = string.Empty;

    [Display(Name = "Mô tả")]
    public string? MoTa { get; set; }

    [StringLength(500)]
    [Display(Name = "Hình ảnh")]
    public string? HinhAnh { get; set; }

    /// <summary>Chuỗi tiện nghi: Wifi, Điều hòa, TV...</summary>
    [Display(Name = "Tiện nghi")]
    public string? TienNghi { get; set; }

    /// <summary>Còn trống / Đang sử dụng / Bảo trì</summary>
    [Required]
    [StringLength(30)]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = HangSo.TrangThaiPhongConTrong;

    public ICollection<DatPhong> DanhSachDatPhong { get; set; } = new List<DatPhong>();
}
