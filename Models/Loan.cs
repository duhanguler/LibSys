using System;

namespace LibSys.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int ResourceId { get; set; }
        public string ResourceTitle { get; set; }
        public Resource Resource { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }

    }
}
