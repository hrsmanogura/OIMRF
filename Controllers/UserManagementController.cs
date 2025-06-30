// Controller: UserManagementController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using OIMRF.Models;
using OIMRF.Services.Audit;
using OIMRF.ViewModels;
using OIMRF.Properties.Data;
using OIMRF.Services.Auth;

namespace OIMRF.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IAuditService _auditService;
        private const string MODULE_NAME = "User Management";
        private readonly IEmailService _emailService;

        public UserManagementController(AppDbContext context, IEmailService emailService, IAuditService auditService)
        {
            _context = context;
            _emailService = emailService;
            _auditService = auditService;
        }
        public IActionResult Index()
        {
            var users = _context.Users
                .Include(u => u.Role) // <-- Important
                .ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.RoleID.ToString(),
                Text = r.RoleName
            }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.Select(r => new SelectListItem
                {
                    Value = r.RoleID.ToString(),
                    Text = r.RoleName
                }).ToList();
                return View(model);
            }

            // Sanitize Contact Number to 09123456789 format
            string sanitizedNumber = model.ContactNumber?.Trim() ?? "";
            if (sanitizedNumber.StartsWith("+63"))
                sanitizedNumber = "0" + sanitizedNumber.Substring(3);
            else if (sanitizedNumber.StartsWith("63"))
                sanitizedNumber = "0" + sanitizedNumber.Substring(2);

            sanitizedNumber = new string(sanitizedNumber.Where(char.IsDigit).ToArray());

            // Create user
            var newUser = new User
            {
                UserID = Guid.NewGuid(),
                Email = model.Email,
                Password = model.Password, // Hash if needed
                FirstName = model.FirstName,
                MiddleName = model.MiddleName,
                LastName = model.LastName,
                Suffix = model.Suffix,
                ContactNumber = sanitizedNumber,
                RoleId = model.RoleId,
                IsVerified = false,
                IsActivated = false,
                LastActivityDate = DateTime.Now
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            _auditService.LogAudit(
                newUser.UserID,
                "User",
                "Created user",
                null,
                "User account created",
                MODULE_NAME
            );

            TempData["SuccessMessage"] = "User created successfully!";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return NotFound();

            var model = new CreateUserViewModel
            {
                UserID = user.UserID,
                Email = user.Email,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Suffix = user.Suffix,
                ContactNumber = user.ContactNumber,
                RoleId = user.RoleId
            };

            ViewBag.Roles = _context.Roles.Select(r => new SelectListItem
            {
                Value = r.RoleID.ToString(),
                Text = r.RoleName
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.Select(r => new SelectListItem
                {
                    Value = r.RoleID.ToString(),
                    Text = r.RoleName
                }).ToList();

                return View(model);
            }

            var user = _context.Users.Find(model.UserID);
            if (user == null) return NotFound();

            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.Suffix = model.Suffix;
            user.ContactNumber = model.ContactNumber;
            user.RoleId = model.RoleId;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleActivation(Guid id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null)
                return NotFound();

            user.IsActivated = !user.IsActivated;

            if (user.IsActivated)
            {
                // Generate OTP
                var otp = new Random().Next(100000, 999999).ToString();

                // Set OTP as temporary password
                user.Password = otp;

                // Build secure link
                var encodedEmail = Uri.EscapeDataString(user.Email);
                var resetLink = Url.Action("ChangePassword", "Home", new { email = encodedEmail }, Request.Scheme);

                // Email body with styled button
                var emailBody = $@"
            <p>Thank you for registering.</p>
            <p>Your one-time password is: <strong>{otp}</strong></p>
            <p>
                <a href='{resetLink}' style='
                    display:inline-block;
                    padding:10px 20px;
                    background-color:#007bff;
                    color:#ffffff;
                    text-decoration:none;
                    border-radius:5px;
                    font-weight:bold;
                '>Click here to set your password</a>
            </p>
            <p>If you didn’t request this, you can ignore this email.</p>";

                // Send the email
                _emailService.SendEmailAsync(
                    user.Email,
                    "Account Activated - Set Your Password",
                    emailBody
                );

                // Log the action
                _auditService.LogAudit(
                    user.UserID,
                    "User",
                    "Activated user",
                    null,
                    $"User {user.Email} activated. OTP sent with password setup link.",
                    MODULE_NAME
                );
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}