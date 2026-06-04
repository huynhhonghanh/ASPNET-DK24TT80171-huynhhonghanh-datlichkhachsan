using DatPhongKhachSanVungTau.Models;

namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Trang chi tiết phòng kèm form đặt phòng.
/// </summary>
public class ChiTietPhongViewModel
{
    public Phong Phong { get; set; } = null!;
    public DatPhongFormViewModel FormDatPhong { get; set; } = new();
    public bool ConTrong { get; set; }
    public bool DaDangNhap { get; set; }
}
