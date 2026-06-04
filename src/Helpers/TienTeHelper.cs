using System.Globalization;

namespace DatPhongKhachSanVungTau.Helpers;

/// <summary>
/// Định dạng tiền VNĐ (VD: 1.500.000 ₫).
/// </summary>
public static class TienTeHelper
{
    private static readonly CultureInfo VnCulture = CultureInfo.GetCultureInfo("vi-VN");

    public static string DinhDangVnd(decimal soTien)
    {
        return string.Format(VnCulture, "{0:N0} ₫", soTien);
    }
}
