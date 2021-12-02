using Microsoft.AspNetCore.Mvc;

namespace POSWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
