using System.Diagnostics;
using System.Globalization;
using BGTG.Web.Controllers.API.BGTG;
using BGTG.Web.Infrastructure.QueryParams;
using BGTG.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConstructionObjectsController _constructionObjectsController;

        public HomeController(ConstructionObjectsController constructionObjectsController)
        {
            _constructionObjectsController = constructionObjectsController;
        }

        public IActionResult Index(DefaultPagedListQueryParams queryParams)
        {
            var operation = _constructionObjectsController.GetPaged(queryParams).Value;

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            return View(operation);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
