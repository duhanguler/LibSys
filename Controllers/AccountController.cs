using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LibSys.Models;
using LibSys.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibSys.Data;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;


    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var loans = _context.Loans
            .Where(l => l.UserId == user.Id)
            .Include(l => l.Resource)
            .ToList();

        var viewModel = new List<UserProfileViewModel>
    {
        new UserProfileViewModel
        {
            UserName = $"{user.FirstName} {user.LastName}",
            Email = user.Email,
            Loans = loans
        }
    };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TCIdentityNumber = model.TCIdentityNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        var model = new LoginViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        
        if (ModelState.IsValid)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);
            // Aşağıdaki kodu aktif hale getirmeyin, giriş yapan kullanıcıyı admin yapar!!
            /*await _userManager.AddToRoleAsync(user, "Admin");*/
            if (user != null)
            {

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
                 
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Index));
    }
}
