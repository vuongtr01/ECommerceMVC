using System;
using System.ComponentModel.DataAnnotations;
namespace ECommerceMVC.ViewModels
{
	public class LoginVM
	{
		[Display(Name = "Username")]
		[Required(ErrorMessage = "*")]
		[MaxLength(20, ErrorMessage = "Maxixum 20 characters")]
		public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
		[DataType(DataType.Password)]
        public string Password { get; set; }
	}
}

