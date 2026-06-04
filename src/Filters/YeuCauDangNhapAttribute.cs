using DatPhongKhachSanVungTau.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatPhongKhachSanVungTau.Filters;

/// <summary>
/// Bắt buộc đăng nhập (dựa trên Session).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class YeuCauDangNhapAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var maNguoiDung = context.HttpContext.Session.GetString(PhienDangNhap.KhoaMaNguoiDung);
        if (string.IsNullOrWhiteSpace(maNguoiDung))
        {
            var req = context.HttpContext.Request;
            var returnUrl = req.Path + req.QueryString;
            context.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", new { returnUrl = returnUrl.ToString() });
        }
    }
}

