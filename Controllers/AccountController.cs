using ManchesterUnitedApp.Data.Entities;
using ManchesterUnitedApp.Models.Account;
using ManchesterUnitedApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ManchesterUnitedApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IEmailSender _emailSender;
        private readonly IEmailService  _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            _emailService = emailService;
            //_emailSender = emailSender;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User users = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }
            }
            return View(model);
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ForgotPasswordConfirmation");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = Url.Action(
                "ResetPassword", "Account",
                new { token, email = model.Email },
                Request.Scheme);

            await _emailService.SendEmailAsync(model.Email,
                "Reset Your Password",
                $"Click the link to reset your password: {resetLink}");

            return RedirectToAction("ForgotPasswordConfirmation");
        }
        // AccountController.cs
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            // This action simply returns the view file associated with its name
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                return BadRequest();

            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirmation");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            // This action simply returns the view that confirms the password 
            // reset process was successful.
            return View();
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }
            var user=await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", " User not found");
                return View(model);
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("ChangePassword", "Account", new { email = model.Email, token = resetToken },Request.Scheme);

            var subject = "Reset Password";
            var body = $"Please reset your password by clicking here:<a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(model.Email,subject,body);
            return RedirectToAction("EmailSent", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }

            var model = new ChangePasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
            
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                ModelState.AddModelError("", "User not found!");
                return View(model);
                }
                var resetResult = await _userManager.RemovePasswordAsync(user);
            if (!resetResult.Succeeded)
            {
                foreach(var error in resetResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
               
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
            
        }

        [HttpGet]
        public IActionResult EmailSent()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}