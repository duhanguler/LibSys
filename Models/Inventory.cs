using System.ComponentModel.DataAnnotations;

namespace LibSys.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }
        public Resource Resource { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Mevcut adet değeri geçerli değil.")]
        public int Quantity { get; set; }

        // Diğer özellikler
    }
}
