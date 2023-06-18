using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibSys.Data;
using LibSys.Models;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme kütüphanesini ekleyin

namespace LibSys.Controllers
{
     // Buraya tüm kullanıcı rolüne sahipler erişebilir.
    public class ResourceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string searchString)
        {
            IQueryable<LibSys.Models.Resource> resources = _context.Resource;

            if (!string.IsNullOrEmpty(searchString))
            {
                resources = resources.Where(r =>
                    EF.Functions.Like(r.Title, $"%{searchString}%") ||
                    EF.Functions.Like(r.Author, $"%{searchString}%") ||
                    EF.Functions.Like(r.Publisher, $"%{searchString}%") ||
                    EF.Functions.Like(r.ISBN, $"%{searchString}%"));
                ViewBag.CurrentFilter = searchString;
            }
            else
            {
                ViewBag.CurrentFilter = null;
            }

            return View(resources.ToList());
        }



        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                _context.Resource.Add(resource);
                _context.SaveChanges();

                // Inventory kaydını oluştur
                var inventory = new Inventory
                {
                    ResourceId = resource.Id,
                    Quantity = 1 // Başlangıçta bir adet
                };
                _context.Inventory.Add(inventory);
                _context.SaveChanges();


                return RedirectToAction("Index");
            }

            // Kaydetme işlemi başarısız olduğunda hata mesajlarını ekrana yansıtmak için ModelState üzerinde işlem yapılmalıdır.
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                ModelState.AddModelError("", error.ErrorMessage);
            }

            return View(resource);
        }

        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Edit(int id)
        {
            var resource = _context.Resource.FirstOrDefault(r => r.Id == id);
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Resource resource)
        {
            if (id != resource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingResource = _context.Resource.FirstOrDefault(r => r.Id == id);
                var existingInventory = _context.Inventory.FirstOrDefault(i => i.ResourceId == id);
                if (existingResource == null)
                {
                    return NotFound();
                }

                existingResource.Quantity = resource.Quantity;
                existingInventory.Quantity = resource.Quantity;
                _context.Entry(existingResource).CurrentValues.SetValues(resource);
                _context.Entry(existingInventory).State = EntityState.Modified;
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault());
            return View(resource);
        }

        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult Delete(int id)
        {
            var resource = _context.Resource.FirstOrDefault(r => r.Id == id);
            if (resource == null)
            {
                return NotFound();
            }
            return View(resource);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var resource = _context.Resource.FirstOrDefault(r => r.Id == id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resource.Remove(resource);

            // Inventory kaydını bul ve sil
            var inventory = _context.Inventory.FirstOrDefault(i => i.ResourceId == id);
            if (inventory != null)
            {
                _context.Inventory.Remove(inventory);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var resource = _context.Resource
                .Include(r => r.Loans)
                .ThenInclude(l => l.User)
                .FirstOrDefault(r => r.Id == id);

            if (resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }




    }
}
