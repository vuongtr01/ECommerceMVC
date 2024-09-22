using System;
using System.ComponentModel.DataAnnotations;
namespace ECommerceMVC.ViewModels
{
	public class RegisterVM
	{
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters")]
        public string MaKh { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "FullName")]
        [Required(ErrorMessage = "*")]
        [MaxLength(50, ErrorMessage = "Maximum 50 characters")]
        public string HoTen { get; set; }

        public bool GioiTinh { get; set; } = true;


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Address")]
        [MaxLength(60, ErrorMessage = "Maximum 60 characters")]
        public string DiaChi { get; set; }

        [Display(Name = "Phone")]
        [MaxLength(24, ErrorMessage = "Maximum 24 characters")]
        [RegularExpression(@"^\d+$", ErrorMessage = "not a phone number format")]
        public string DienThoai { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Not a correct email address")]
        public string Email { get; set; }
    }
}

