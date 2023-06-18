using System;
using System.ComponentModel.DataAnnotations;

namespace LibSys.Models.ViewModels
{
    public class BorrowViewModel
    {
        public int LoanId { get; set; }
        public Loan Loan { get; set; }

        public int ResourceId { get; set; }
        public string ResourceTitle { get; set; }
        public Resource Resource { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "Borrow Date")]
        public DateTime BorrowDate { get; set; }

        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
    }
}
