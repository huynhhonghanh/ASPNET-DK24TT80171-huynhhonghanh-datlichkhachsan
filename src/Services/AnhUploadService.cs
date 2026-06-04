namespace DatPhongKhachSanVungTau.Services;

/// <summary>
/// Upload ảnh vào wwwroot/images.
/// </summary>
public class AnhUploadService
{
    private readonly IWebHostEnvironment _env;
    private static readonly string[] DinhDangHopLe = { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg" };

    public AnhUploadService(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// Lưu file ảnh vào thư mục con (vd: khachsan, phong). Trả về đường dẫn web (/images/...).
    /// </summary>
    public async Task<string?> LuuAnhAsync(IFormFile? file, string thuMuc)
    {
        if (file == null || file.Length == 0)
        {
            return null;
        }

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!DinhDangHopLe.Contains(ext))
        {
            return null;
        }

        var folder = Path.Combine(_env.WebRootPath, "images", thuMuc);
        Directory.CreateDirectory(folder);

        var tenFile = $"{Guid.NewGuid():N}{ext}";
        var duongDanVatLy = Path.Combine(folder, tenFile);

        await using var stream = new FileStream(duongDanVatLy, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/images/{thuMuc}/{tenFile}";
    }

    /// <summary>
    /// Xóa ảnh cũ nếu nằm trong wwwroot/images.
    /// </summary>
    public void XoaAnhCu(string? duongDanWeb)
    {
        if (string.IsNullOrWhiteSpace(duongDanWeb) || !duongDanWeb.StartsWith("/images/"))
        {
            return;
        }

        var duongDanVatLy = Path.Combine(_env.WebRootPath, duongDanWeb.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(duongDanVatLy))
        {
            File.Delete(duongDanVatLy);
        }
    }
}
