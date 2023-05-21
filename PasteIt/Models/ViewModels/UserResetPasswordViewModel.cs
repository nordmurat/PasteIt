using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PasteIt.Models.ViewModels
{
	public class UserResetPasswordViewModel
	{
        [Display(Name = "E-Posta Adresiniz")]
        [Required(ErrorMessage = "Lütfen e-posta adresinizi boş geçmeyiniz.")]
        [EmailAddress(ErrorMessage = "Lütfen uygun formatta e-posta giriniz.")]
        public string Email { get; set; }
    }
}

