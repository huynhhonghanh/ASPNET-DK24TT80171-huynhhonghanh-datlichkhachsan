namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Thông tin khách sạn hiển thị trên trang chủ.
/// </summary>
public class KhachSanNoiBatViewModel
{
    public int MaKhachSan { get; set; }
    public string TenKhachSan { get; set; } = string.Empty;
    public string KhuVuc { get; set; } = string.Empty;
    public string? HinhAnh { get; set; }
    public int XepHang { get; set; }
    public int KhoangCachBien { get; set; }
    public decimal GiaTu { get; set; }
    public int SoPhong { get; set; }
}
