using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using LibSys.Models;

namespace LibSys.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Hata ayrıntılarını yazdır
                var errorMsg = ex.Message;
                // İlgili hata mesajlarını daha ayrıntılı bir şekilde loglama veya raporlama yapabilirsiniz
                throw; // Hatanın yukarıya fırlatılması
            }
        }
    }
}
