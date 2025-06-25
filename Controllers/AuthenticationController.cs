using Microsoft.AspNetCore.Mvc;

namespace OIMRF.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
