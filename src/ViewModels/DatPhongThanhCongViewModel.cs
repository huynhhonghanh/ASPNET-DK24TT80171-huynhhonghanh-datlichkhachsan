namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Thông tin hiển thị sau khi đặt phòng thành công.
/// </summary>
public class DatPhongThanhCongViewModel
{
    public int MaDatPhong { get; set; }
    public string TenKhachSan { get; set; } = string.Empty;
    public string SoPhong { get; set; } = string.Empty;
    public DateTime NgayNhanPhong { get; set; }
    public DateTime NgayTraPhong { get; set; }
    public int SoDem { get; set; }
    public int SoKhach { get; set; }
    public decimal TongTien { get; set; }
    public string PhuongThucThanhToan { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayDat { get; set; }
}
