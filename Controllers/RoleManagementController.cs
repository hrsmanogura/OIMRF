using Microsoft.AspNetCore.Mvc;
using OIMRF.Models;
using OIMRF.Properties.Data;
using System;
using System.Linq;

namespace OIMRF.Controllers
{
    public class RoleManagementController : Controller
    {
        private readonly AppDbContext _context;

        public RoleManagementController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public IActionResult Index()
        {
            var roles = _context.Roles.OrderBy(r => r.RoleName).ToList();
            return View(roles);
        }

        // POST: Role/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            role.RoleID = Guid.NewGuid();
            _context.Roles.Add(role);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Role added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Role/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var existing = _context.Roles.FirstOrDefault(r => r.RoleID == role.RoleID);
            if (existing == null)
                return NotFound();

            existing.RoleName = role.RoleName;
            existing.Description = role.Description;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Role updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}