using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme kütüphanesini ekleyin
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibSys.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            // İstatistikleri hesapla
            var totalResources = _context.Resource.Count();
            var totalInventoryItems = _context.Inventory.Sum(i => i.Quantity);
            var borrowedItems = _context.Loans.Count();

            // View'e verileri gönder
            ViewBag.TotalResources = totalResources;
            ViewBag.TotalInventoryItems = totalInventoryItems;
            ViewBag.BorrowedItems = borrowedItems;

            return View();
        }
    }
}
