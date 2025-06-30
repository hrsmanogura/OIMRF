using Microsoft.AspNetCore.Mvc;
using OIMRF.Models;
using OIMRF.Properties.Data;
using OIMRF.ViewModels;
using System;
using System.Linq;

namespace OIMRF.Controllers
{
    public class PermissionController : Controller
    {
        private readonly AppDbContext _context;

        public PermissionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = _context.Roles
                .Select(r => new RoleWithPermissionsViewModel
                {
                    RoleID = r.RoleID,  
                    RoleName = r.RoleName,
                    Description = r.Description,

                    CanCreate = _context.RolePermission.Any(p => p.RoleID == r.RoleID && p.CanCreate),
                    CanRead = _context.RolePermission.Any(p => p.RoleID == r.RoleID && p.CanRead),
                    CanUpdate = _context.RolePermission.Any(p => p.RoleID == r.RoleID && p.CanUpdate),
                    CanDelete = _context.RolePermission.Any(p => p.RoleID == r.RoleID && p.CanDelete),
                })
                .ToList();

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Permission permission)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            permission.PermissionID = Guid.NewGuid();
            _context.Permission.Add(permission);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Permission added successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Permission permission)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var existing = _context.Permission.FirstOrDefault(p => p.PermissionID == permission.PermissionID);
            if (existing == null)
                return NotFound();

            existing.Name = permission.Name;
            existing.Description = permission.Description;
            existing.CanCreate = permission.CanCreate;
            existing.CanRead = permission.CanRead;
            existing.CanUpdate = permission.CanUpdate;
            existing.CanDelete = permission.CanDelete;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Permission updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
