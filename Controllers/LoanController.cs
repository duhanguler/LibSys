using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;
using LibSys.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LibSys.Controllers
{
    public class LoanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoanController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Borrow(BorrowViewModel model)
        {
            var resource = _context.Resource.FirstOrDefault(r => r.Id == model.ResourceId);
            var user = await _userManager.GetUserAsync(User);

            if (resource == null || user == null || !resource.IsAvailableForBorrowing)
            {
                return BadRequest("The resource is not available for borrowing or user/resource does not exist.");
            }

            var loan = new Loan
            {
                UserId = user.Id, // Giriş yapan kullanıcının UserId'sini kullan
                ResourceId = model.ResourceId,
                ResourceTitle = resource.Title,
                BorrowDate = DateTime.Now,
                ReturnDate = DateTime.Now.AddDays(resource.LoanPeriod ?? 0), // İade tarihi LoanPeriod gün sonra
            };
            var inventory = _context.Inventory.FirstOrDefault(i => i.ResourceId == loan.ResourceId);
            if (inventory.Quantity > 1)
            {
                resource.IsAvailableForBorrowing = true; // Kaynağı ödünç alındı olarak işaretle
                inventory.Quantity--;
            }
            else if (inventory.Quantity == 1)
            {
                resource.IsAvailableForBorrowing = false;
                inventory.Quantity--;
            }
            else
            { 
                resource.IsAvailableForBorrowing = false;
                return BadRequest("The resource is not available for borrowing.");
            }

            _context.Loans.Add(loan);
            _context.SaveChanges();

            return RedirectToAction("Index", "Resource");
        }


        [HttpPost]
        public IActionResult Return(int loanId)
        {
            var loan = _context.Loans.Include(l => l.Resource).FirstOrDefault(l => l.LoanId == loanId);

            if (loan == null)
            {
                return NotFound();
            }

            loan.ReturnDate = DateTime.Now;

            var inventory = _context.Inventory.FirstOrDefault(i => i.ResourceId == loan.ResourceId);
            if (inventory != null)
            {

                var resource = _context.Resource.FirstOrDefault(r => r.Id == loan.ResourceId);
                if (resource != null && inventory.Quantity >= 0)
                {
                    resource.IsAvailableForBorrowing = true;
                    inventory.Quantity++;
                    loan.ReturnDate = DateTime.Now;
                    _context.Entry(resource).State = EntityState.Modified;
                }
            }

            // Kredi kullanıcının krediler listesinden kaldırılır
            var user = _userManager.GetUserAsync(User).Result;
            var userLoans = _context.Loans.Where(l => l.UserId == user.Id).ToList();
            var userLoan = userLoans.FirstOrDefault(l => l.LoanId == loanId);
            if (userLoan != null)
            {
                _context.Loans.Remove(userLoan);
                _context.SaveChanges();
            }

            TempData["SuccessMessage"] = "İade işlemi başarıyla tamamlandı.";

            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult QuickReturn(string userId)
        {
            var loans = _context.Loans.Where(l => l.UserId == userId && l.ReturnDate == null).ToList();

            foreach (var loan in loans)
            {
                loan.ReturnDate = DateTime.Now;

                var inventory = _context.Inventory.FirstOrDefault(i => i.ResourceId == loan.ResourceId);
                if (inventory != null)
                {
                    inventory.Quantity++;

                    var resource = _context.Resource.FirstOrDefault(r => r.Id == loan.ResourceId);
                    if (resource != null && inventory.Quantity > 0)
                    {
                        resource.IsAvailableForBorrowing = true;
                        _context.Entry(resource).State = EntityState.Modified;
                    }
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Tüm kaynakların iade işlemi başarıyla tamamlandı.";
            return RedirectToAction("Details", "User", new { id = userId });
        }





    }
}
