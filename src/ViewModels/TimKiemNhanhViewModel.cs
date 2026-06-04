using System.ComponentModel.DataAnnotations;

namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Form tìm kiếm nhanh trên trang chủ.
/// </summary>
public class TimKiemNhanhViewModel
{
    [Display(Name = "Khu vực bãi biển")]
    public string? KhuVuc { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn ngày nhận phòng")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhận phòng")]
    public DateTime NgayNhanPhong { get; set; } = DateTime.Today.AddDays(1);

    [Required(ErrorMessage = "Vui lòng chọn ngày trả phòng")]
    [DataType(DataType.Date)]
    [Display(Name = "Ngày trả phòng")]
    public DateTime NgayTraPhong { get; set; } = DateTime.Today.AddDays(2);

    [Range(1, 20, ErrorMessage = "Số khách từ 1 đến 20")]
    [Display(Name = "Số khách")]
    public int SoKhach { get; set; } = 2;
}
