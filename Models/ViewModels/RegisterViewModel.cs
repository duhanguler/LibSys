using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LibSys.Models.ViewModels
{

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email alanı gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        [Compare("Password",
            ErrorMessage = "Şifre ve onay şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "TC Kimlik Numarası alanı gereklidir.")]
        public string TCIdentityNumber { get; set; }
    }

}
