using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers
{
    public class PosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
