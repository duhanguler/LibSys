using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibSys.Models.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "TC Identity Number")]
        public string TCIdentityNumber { get; set; }

        // User's roles
        public List<string> UserRoles { get; set; }

        // All available roles
        public IEnumerable<string>? AllRoles { get; set; }

    }
}
