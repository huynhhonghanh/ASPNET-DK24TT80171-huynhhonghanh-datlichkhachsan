namespace DatPhongKhachSanVungTau.ViewModels;

public class LichSuDatPhongViewModel
{
    public List<DatPhongLichSuItemViewModel> DanhSach { get; set; } = new();
    public string? LocTrangThai { get; set; }
}
