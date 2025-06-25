using Microsoft.AspNetCore.Mvc;

namespace OIMRF.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            // Static severity data per entity
            var entitySeverityData = new List<object>
            {
                new { Entity = "DENR", High = 12, Medium = 18, Low = 6 },
                new { Entity = "DPWH", High = 5, Medium = 22, Low = 15 },
                new { Entity = "LGU", High = 8, Medium = 12, Low = 20 }
            };

            // Static response time data (in days) per region
            var responseTimeData = new Dictionary<string, double>
            {
                { "NCR", 1.8 },
                { "Region I (Ilocos)", 3.5 },
                { "Region II (Cagayan Valley)", 2.2 },
                { "Region III (Central Luzon)", 4.1 },
                { "Region IV-A (CALABARZON)", 3.2 },
                { "MIMAROPA", 3.9 },
                { "Region V (Bicol)", 2.8 },
                { "Region VI (Western Visayas)", 3.6 },
                { "Region VII (Central Visayas)", 2.9 },
                { "Region VIII (Eastern Visayas)", 4.3 },
                { "Region IX (Zamboanga Peninsula)", 4.0 },
                { "Region X (Northern Mindanao)", 3.1 },
                { "Region XI (Davao Region)", 2.7 },
                { "Region XII (SOCCSKSARGEN)", 3.0 },
                { "Region XIII (Caraga)", 3.4 },
                { "BARMM", 5.0 },
                { "CAR (Cordillera)", 2.5 }
            };

            // Root cause distribution
            var complaintNatureData = new Dictionary<string, int>
            {
                { "Human Factors", 32 },
                { "Process Issues", 18 },
                { "Technical Issues", 25 },
                { "Environmental Factors", 42 },
                { "Stakeholder Issues", 29 }
            };

            // Complaints per region and agency (static, for demonstration)
            var regionAgencyData = new List<object>
            {
                new { Region = "NCR", Agency = "DENR", Complaints = 34 },
                new { Region = "Region I (Ilocos)", Agency = "DPWH", Complaints = 21 },
                new { Region = "Region II (Cagayan Valley)", Agency = "LGU", Complaints = 17 },
                new { Region = "Region III (Central Luzon)", Agency = "DENR", Complaints = 25 },
                new { Region = "Region IV-A (CALABARZON)", Agency = "DENR", Complaints = 27 },
                new { Region = "MIMAROPA", Agency = "LGU", Complaints = 12 },
                new { Region = "Region V (Bicol)", Agency = "DPWH", Complaints = 18 },
                new { Region = "Region VI (Western Visayas)", Agency = "DENR", Complaints = 19 },
                new { Region = "Region VII (Central Visayas)", Agency = "LGU", Complaints = 23 },
                new { Region = "Region VIII (Eastern Visayas)", Agency = "DPWH", Complaints = 16 },
                new { Region = "Region IX (Zamboanga Peninsula)", Agency = "DENR", Complaints = 11 },
                new { Region = "Region X (Northern Mindanao)", Agency = "LGU", Complaints = 14 },
                new { Region = "Region XI (Davao Region)", Agency = "DENR", Complaints = 20 },
                new { Region = "Region XII (SOCCSKSARGEN)", Agency = "DPWH", Complaints = 22 },
                new { Region = "Region XIII (Caraga)", Agency = "LGU", Complaints = 15 },
                new { Region = "BARMM", Agency = "DENR", Complaints = 10 },
                new { Region = "CAR (Cordillera)", Agency = "LGU", Complaints = 13 }
            };

            ViewBag.EntitySeverityData = entitySeverityData;
            ViewBag.ResponseTimeData = responseTimeData;
            ViewBag.ComplaintNatureData = complaintNatureData;
            ViewBag.RegionAgencyData = regionAgencyData;

            return View();
        }
    }
}
