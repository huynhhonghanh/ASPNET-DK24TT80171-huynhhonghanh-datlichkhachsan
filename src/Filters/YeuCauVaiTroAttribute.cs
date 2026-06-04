using DatPhongKhachSanVungTau.Models;
using DatPhongKhachSanVungTau.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatPhongKhachSanVungTau.Filters;

/// <summary>
/// Bắt buộc đúng vai trò (Admin/KhachHang) dựa trên Session.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class YeuCauVaiTroAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _vaiTro;

    public YeuCauVaiTroAttribute(string vaiTro)
    {
        _vaiTro = vaiTro;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var maNguoiDung = context.HttpContext.Session.GetString(PhienDangNhap.KhoaMaNguoiDung);
        if (string.IsNullOrWhiteSpace(maNguoiDung))
        {
            context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", new { returnUrl = context.HttpContext.Request.Path.ToString() });
            return;
        }

        var vaiTro = context.HttpContext.Session.GetString(PhienDangNhap.KhoaVaiTro);
        if (!string.Equals(vaiTro, _vaiTro, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status403Forbidden,
                ContentType = "text/plain; charset=utf-8",
                Content = $"Bạn không có quyền truy cập. Yêu cầu vai trò: {_vaiTro}."
            };
        }
    }
}

public static class YeuCauVaiTro
{
    public static YeuCauVaiTroAttribute Admin => new(HangSo.VaiTroAdmin);
    public static YeuCauVaiTroAttribute KhachHang => new(HangSo.VaiTroKhachHang);
}

