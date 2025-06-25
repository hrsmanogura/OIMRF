using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OIMRF.Models;
using OIMRF.Properties.Data;
using System.Linq;

namespace OIMRF.Controllers
{
    public class AuditTrailController : Controller
    {
        private readonly AppDbContext _context;

        public AuditTrailController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var audits = _context.AuditTrail
                .OrderByDescending(a => a.CreatedDate)
                .ToList();

            return View(audits);
        }
    }
}
