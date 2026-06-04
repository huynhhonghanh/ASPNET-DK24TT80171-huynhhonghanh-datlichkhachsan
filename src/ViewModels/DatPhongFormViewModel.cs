using System.ComponentModel.DataAnnotations;

namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Form đặt phòng online.
/// </summary>
public class DatPhongFormViewModel
{
    [Required]
    public int MaPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày nhận phòng")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhận phòng")]
    public DateTime NgayNhanPhong { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày trả phòng")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày trả phòng")]
    public DateTime NgayTraPhong { get; set; }

    [Range(1, 20, ErrorMessage = "Số khách từ 1 đến 20")]
    [Display(Name = "Số khách")]
    public int SoKhach { get; set; } = 2;

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
    [Display(Name = "Phương thức thanh toán")]
    public string PhuongThucThanhToan { get; set; } = Models.HangSo.ThanhToanTienMat;

    // Hiển thị (không post từ client — tính lại server)
    public string? TenKhachSan { get; set; }
    public string? SoPhong { get; set; }
    public string? TenLoaiPhong { get; set; }
    public int SoNguoiToiDa { get; set; }
    public decimal GiaMotDem { get; set; }
    public int SoDem { get; set; }
    public decimal TongTien { get; set; }
    public string? HinhAnh { get; set; }
}
