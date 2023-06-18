using System.Collections.Generic;
using LibSys.Models;

namespace LibSys.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TCIdentityNumber { get; set; }
        public List<Loan> Loans { get; set; }
    }
}
