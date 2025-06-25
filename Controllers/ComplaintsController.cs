// CONTROLLER: ComplaintsController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OIMRF.Models;
using OIMRF.Properties.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OIMRF.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly AppDbContext _context;

        public ComplaintsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints.ToListAsync();
            return View(complaints);
        }

        public IActionResult Create()
        {
            var model = new Complaints
            {
                ComplaintID = GenerateNextComplaintID(),
                ComplaintGUID = Guid.NewGuid()
            };
            ViewBag.CurrentStep = 1;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Complaints complaint, int CurrentStep)
        {
            if (ModelState.IsValid)
            {
                complaint.ComplaintID = GenerateNextComplaintID();
                complaint.ComplaintGUID = Guid.NewGuid();
                complaint.CreatedAt = DateTime.Now;

                _context.Add(complaint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CurrentStep = CurrentStep;
            return View(complaint);
        }

        private string GenerateNextComplaintID()
        {
            var last = _context.Complaints
                .OrderByDescending(c => c.ComplaintID)
                .Select(c => c.ComplaintID)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(last) || !last.StartsWith("C-"))
                return "C-0000000001";

            var number = int.Parse(last.Substring(2));
            return $"C-{(number + 1).ToString().PadLeft(10, '0')}";
        }

        private bool ComplaintExists(Guid id)
        {
            return _context.Complaints.Any(e => e.ComplaintGUID == id);
        }
    }
}
