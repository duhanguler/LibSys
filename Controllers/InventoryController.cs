using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme kütüphanesini ekleyin
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibSys.Controllers
{
    [Authorize(Roles = "Admin,Librarian")]
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var inventory = _context.Inventory.Include(i => i.Resource).ToList();
            return View(inventory);
        }

        public IActionResult Create()
        {
            PopulateResourceDropDownList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Inventory.Add(inventory);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateResourceDropDownList(inventory.ResourceId);
            return View(inventory);
        }

        public IActionResult Edit(int id)
        {
            var inventory = _context.Inventory.FirstOrDefault(i => i.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            var resourceTitle = _context.Resource.FirstOrDefault(r => r.Id == inventory.ResourceId)?.Title;
            ViewBag.ResourceTitle = resourceTitle;

            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(inventory);
                _context.SaveChanges();
                return RedirectToAction("Details", "Resource", new { id = inventory.ResourceId });
            }

            var resourceTitle = _context.Resource.FirstOrDefault(r => r.Id == inventory.ResourceId)?.Title;
            ViewBag.ResourceTitle = resourceTitle;

            return View(inventory);
        }

        public IActionResult Delete(int id)
        {
            var inventory = _context.Inventory.FirstOrDefault(i => i.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }
            return View(inventory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var inventory = _context.Inventory.FirstOrDefault(i => i.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            _context.Inventory.Remove(inventory);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private void PopulateResourceDropDownList(int? selectedResourceId = null)
        {
            var resources = _context.Resource.Select(r => new
            {
                Id = r.Id,
                Title = r.Title
            }).ToList();

            ViewBag.ResourceId = new SelectList(resources, "Id", "Title", selectedResourceId);
        }
    }
}
