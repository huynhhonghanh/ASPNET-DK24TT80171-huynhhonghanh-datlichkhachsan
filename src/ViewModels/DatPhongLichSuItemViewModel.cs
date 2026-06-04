namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Một đơn đặt phòng trong lịch sử.
/// </summary>
public class DatPhongLichSuItemViewModel
{
    public int MaDatPhong { get; set; }
    public string TenKhachSan { get; set; } = string.Empty;
    public string KhuVuc { get; set; } = string.Empty;
    public string SoPhong { get; set; } = string.Empty;
    public string? HinhAnh { get; set; }
    public DateTime NgayNhanPhong { get; set; }
    public DateTime NgayTraPhong { get; set; }
    public int SoDem { get; set; }
    public int SoKhach { get; set; }
    public decimal TongTien { get; set; }
    public string PhuongThucThanhToan { get; set; } = string.Empty;
    public string TrangThai { get; set; } = string.Empty;
    public DateTime NgayDat { get; set; }
    public bool CoTheHuy { get; set; }
}
