using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibSys.Models
{
    public class Resource
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı gereklidir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yazar alanı gereklidir.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Yayınevi alanı gereklidir.")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Yayın Tarihi alanı gereklidir.")]
        [DataType(DataType.Date)]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "ISBN alanı gereklidir.")]
        public string ISBN { get; set; }

        public string Description { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>(); // Boş bir liste olarak başlatılıyor
        public bool IsAvailableForBorrowing { get; set; }
        public int? LoanPeriod { get; set; } // Nullable olarak tanımlanıyor

        public int Quantity { get; set; } // Quantity özelliği eklendi

        // Diğer özellikler
    }
}
