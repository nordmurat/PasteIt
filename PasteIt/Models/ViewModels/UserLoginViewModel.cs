using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PasteIt.Models.ViewModels
{
	public class UserLoginViewModel
	{
        [Required(ErrorMessage = "Lütfen kullanıcı adını boş geçmeyiniz...")]
        [StringLength(15, ErrorMessage = "Lütfen kullanıcı adını 4 ile 15 karakter arasında giriniz...", MinimumLength = 4)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Lütfen şifreyi boş geçmeyiniz.")]
        [DataType(DataType.Password, ErrorMessage = "Lütfen uygun formatta şifre giriniz.")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        /// <summary>
        /// Beni hatırla...
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool Persistent { get; set; }
        public bool Lock { get; set; }
    }
}

