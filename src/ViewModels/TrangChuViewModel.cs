namespace DatPhongKhachSanVungTau.ViewModels;

public class TrangChuViewModel
{
    public TimKiemNhanhViewModel TimKiem { get; set; } = new();
    public List<KhachSanNoiBatViewModel> KhachSanNoiBat { get; set; } = new();
}
