using Microsoft.AspNetCore.Mvc;
using OIMRF.Models;
using OIMRF.Properties.Data;
using System;
using System.Linq;

namespace OIMRF.Controllers
{
    public class DesignationController : Controller
    {
        private readonly AppDbContext _context;

        public DesignationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var list = _context.Designation.OrderBy(d => d.Title).ToList();
            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Designation designation)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            designation.DesignationID = Guid.NewGuid();
            _context.Designation.Add(designation);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Designation added successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Designation designation)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            var existing = _context.Designation.FirstOrDefault(d => d.DesignationID == designation.DesignationID);
            if (existing == null)
                return NotFound();

            existing.Title = designation.Title;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Designation updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
