using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PasteIt.Models.ViewModels
{
	public class UserUpdatePasswordViewModel
	{
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Lütfen şifreyi boş geçmeyiniz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

