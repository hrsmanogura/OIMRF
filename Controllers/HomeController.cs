using OIMRF.Models;
using OIMRF.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using OIMRF.Properties.Data;
using OIMRF.Services.Auth;
using Microsoft.AspNetCore.Http;

namespace OIMRF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IEmailService emailService)
        {
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user == null || user.Password != model.Password)
            {
                TempData["ToastError"] = "Invalid username or password.";
                return View(model);
            }

            // Always send OTP for login security
            var otp = GenerateOtp();
            await _emailService.SendEmailAsync(user.Email, "Your OTP Code", $"Your code is: {otp}");

            HttpContext.Session.SetString("LoginOTP", otp);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("PendingPasswordChange", (!user.IsVerified).ToString());

            TempData["ShowOtpModal"] = "true";
            return View(model); // show OTP modal
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new User
            {
                UserID = Guid.NewGuid(),
                Email = model.Email,
                Password = model.Password, // Hash this in production
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Suffix = model.Suffix,
                ContactNumber = model.ContactNumber,
                RoleId = Guid.Parse("B9E25771-35AC-427C-BF3B-91E86944F7D3"),
                IsVerified = false,
                IsActivated = false,
                LastActivityDate = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["RegistrationSuccess"] = "true";
            return RedirectToAction("Register");

        }


        [HttpPost]
        public IActionResult VerifyOtp(string OtpCode)
        {
            var storedOtp = HttpContext.Session.GetString("LoginOTP");
            var email = HttpContext.Session.GetString("UserEmail");
            var forceChange = HttpContext.Session.GetString("PendingPasswordChange") == "True";

            if (storedOtp == null || OtpCode?.Trim() != storedOtp)
            {
                TempData["OtpError"] = "Invalid OTP.";
                TempData["ShowOtpModal"] = "true";
                return RedirectToAction("Index");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Index");

            // Clear session
            HttpContext.Session.Remove("LoginOTP");
            HttpContext.Session.Remove("UserEmail");

            // Login user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Name, user.Email ?? "")
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true });

            if (forceChange)
                return RedirectToAction("ChangePassword", new { email = user.Email });

            return RedirectToAction("Dashboard");
        }


        [HttpPost]
        public IActionResult SetNewPassword(string NewPassword, string ConfirmPassword)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (NewPassword != ConfirmPassword)
            {
                TempData["PasswordError"] = "Passwords do not match.";
                TempData["ShowChangePasswordModal"] = "true";
                return RedirectToAction("Index");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Index");

            user.Password = NewPassword; // hash in production
            user.IsVerified = true;

            _context.SaveChanges();

            // Sign them in
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
        new Claim(ClaimTypes.Name, user.Email ?? ""),
        new Claim("FullName", $"{user.FirstName} {user.LastName}")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = true });

            HttpContext.Session.Remove("LoginOTP");
            HttpContext.Session.Remove("UserEmail");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ChangePassword(string email)
        {
            return View(new ChangePasswordViewModel { Email = email });
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (user == null) return NotFound();

            user.Password = model.NewPassword;
            user.IsVerified = true;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Password changed successfully.";
            return RedirectToAction("Index");
        }



        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
