namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// Hằng số trạng thái và vai trò dùng chung trong hệ thống.
/// </summary>
public static class HangSo
{
    // Vai trò người dùng
    public const string VaiTroAdmin = "Admin";
    public const string VaiTroKhachHang = "KhachHang";

    // Trạng thái tài khoản
    public const string TrangThaiTaiKhoanHoatDong = "HoatDong";
    public const string TrangThaiTaiKhoanBiKhoa = "BiKhoa";

    // Khu vực bãi biển Vũng Tàu
    public static readonly string[] DanhSachKhuVuc =
    {
        "Bãi Trước",
        "Bãi Sau",
        "Bãi Dâu",
        "Bãi Dứa",
        "Bãi Vọng Nguyệt"
    };

    // Trạng thái phòng
    public const string TrangThaiPhongConTrong = "Còn trống";
    public const string TrangThaiPhongDangSuDung = "Đang sử dụng";
    public const string TrangThaiPhongBaoTri = "Bảo trì";

    // Trạng thái đặt phòng
    public const string TrangThaiDatChoXacNhan = "Chờ xác nhận";
    public const string TrangThaiDatDaXacNhan = "Đã xác nhận";
    public const string TrangThaiDatDaNhanPhong = "Đã nhận phòng";
    public const string TrangThaiDatDaTraPhong = "Đã trả phòng";
    public const string TrangThaiDatDaHuy = "Đã hủy";

    // Phương thức thanh toán
    public const string ThanhToanTienMat = "Tiền mặt";
    public const string ThanhToanChuyenKhoan = "Chuyển khoản";
    public const string ThanhToanThe = "Thẻ";

    // Hướng phòng
    public const string HuongViewBien = "View biển";
    public const string HuongViewThanhPho = "View thành phố";
    public const string HuongViewHoBoi = "View hồ bơi";

    /// <summary>Khách hàng được hủy khi chưa nhận phòng.</summary>
    public static bool CoTheHuyDatPhong(string trangThai)
    {
        return trangThai == TrangThaiDatChoXacNhan || trangThai == TrangThaiDatDaXacNhan;
    }
}
