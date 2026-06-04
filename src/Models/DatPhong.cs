using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Đơn đặt phòng của khách hàng.
/// </summary>
[Table("DatPhong")]
public class DatPhong
{
    [Key]
    [Display(Name = "Mã đặt phòng")]
    public int MaDatPhong { get; set; }

    [Display(Name = "Khách hàng")]
    public int MaNguoiDung { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    public NguoiDung? NguoiDung { get; set; }

    [Display(Name = "Phòng")]
    public int MaPhong { get; set; }

    [ForeignKey(nameof(MaPhong))]
    public Phong? Phong { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhận phòng")]
    public DateTime NgayNhanPhong { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày trả phòng")]
    public DateTime NgayTraPhong { get; set; }

    /// <summary>Số đêm lưu trú (tự tính khi đặt)</summary>
    [Display(Name = "Số đêm")]
    public int SoDem { get; set; }

    [Range(1, 20, ErrorMessage = "Số khách từ 1 đến 20")]
    [Display(Name = "Số khách")]
    public int SoKhach { get; set; }

    [Column(TypeName = "decimal(18,0)")]
    [Display(Name = "Tổng tiền (VNĐ)")]
    public decimal TongTien { get; set; }

    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }

    /// <summary>Tiền mặt / Chuyển khoản / Thẻ</summary>
    [Required]
    [StringLength(30)]
    [Display(Name = "Phương thức thanh toán")]
    public string PhuongThucThanhToan { get; set; } = HangSo.ThanhToanTienMat;

    /// <summary>Chờ xác nhận / Đã xác nhận / Đã nhận phòng / Đã trả phòng / Đã hủy</summary>
    [Required]
    [StringLength(30)]
    [Display(Name = "Trạng thái")]
    public string TrangThai { get; set; } = HangSo.TrangThaiDatChoXacNhan;

    [Display(Name = "Ngày đặt")]
    public DateTime NgayDat { get; set; } = DateTime.Now;
}
