using System.ComponentModel.DataAnnotations;
using DatPhongKhachSanVungTau.Models;

namespace DatPhongKhachSanVungTau.ViewModels;

/// <summary>
/// Form tìm kiếm phòng đầy đủ (Bước 6).
/// </summary>
public class TimKiemPhongViewModel
{
    [Display(Name = "Khu vực bãi biển")]
    public string? KhuVuc { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngày nhận phòng")]
    public DateTime NgayNhanPhong { get; set; } = DateTime.Today.AddDays(1);

    [DataType(DataType.Date)]
    [Display(Name = "Ngày trả phòng")]
    public DateTime NgayTraPhong { get; set; } = DateTime.Today.AddDays(2);

    [Range(1, 20)]
    [Display(Name = "Số khách")]
    public int SoKhach { get; set; } = 2;

    [Display(Name = "Loại phòng")]
    public int? MaLoaiPhong { get; set; }

    [Display(Name = "Xếp hạng sao")]
    public int? XepHang { get; set; }

    [Display(Name = "Hướng phòng")]
    public string? HuongPhong { get; set; }

    [Display(Name = "Giá từ (VNĐ/đêm)")]
    public decimal? GiaTu { get; set; }

    [Display(Name = "Giá đến (VNĐ/đêm)")]
    public decimal? GiaDen { get; set; }

    /// <summary>Giá min/max trong DB — dùng cho slider.</summary>
    public decimal GiaMinHeThong { get; set; }
    public decimal GiaMaxHeThong { get; set; }

    public List<LoaiPhong> DanhSachLoaiPhong { get; set; } = new();
    public string[] DanhSachHuongPhong { get; set; } =
    {
        HangSo.HuongViewBien,
        HangSo.HuongViewThanhPho,
        HangSo.HuongViewHoBoi
    };

    public List<PhongTimKiemItemViewModel> KetQua { get; set; } = new();
    public int TongSoKetQua => KetQua.Count;
    public bool DaTimKiem { get; set; }
}
