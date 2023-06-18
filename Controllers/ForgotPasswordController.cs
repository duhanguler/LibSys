using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LibSys.Models;
using LibSys.Models.ViewModels;

namespace LibSys.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Email confirmation is required for password reset.
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ForgotPasswordConfirmation", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);

                // Send the password reset email to the user
                // You can use your preferred email sending method here

                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we reach this point, something went wrong. Show the view with validation errors.
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}
