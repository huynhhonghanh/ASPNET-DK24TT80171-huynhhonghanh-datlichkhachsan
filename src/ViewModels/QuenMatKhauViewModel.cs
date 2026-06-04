using System.ComponentModel.DataAnnotations;

namespace DatPhongKhachSanVungTau.ViewModels;

public class QuenMatKhauViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}

