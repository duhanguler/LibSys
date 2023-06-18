using Microsoft.AspNetCore.Identity;

namespace LibSys.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TCIdentityNumber { get; set; }

    }
}
