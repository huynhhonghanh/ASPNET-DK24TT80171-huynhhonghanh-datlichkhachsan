namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Một phòng trong kết quả tìm kiếm.
/// </summary>
public class PhongTimKiemItemViewModel
{
    public int MaPhong { get; set; }
    public string SoPhong { get; set; } = string.Empty;
    public string TenKhachSan { get; set; } = string.Empty;
    public string KhuVuc { get; set; } = string.Empty;
    public int XepHang { get; set; }
    public int KhoangCachBien { get; set; }
    public string TenLoaiPhong { get; set; } = string.Empty;
    public int SoNguoiToiDa { get; set; }
    public string HuongPhong { get; set; } = string.Empty;
    public string? HinhAnh { get; set; }
    public string? TienNghi { get; set; }
    public decimal GiaMotDem { get; set; }
    public int SoDem { get; set; }
    public decimal TongTien { get; set; }
}
