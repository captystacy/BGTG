using System.Diagnostics;
using System.Globalization;
using BGTG.Web.Controllers.API;
using BGTG.Web.ViewModels;
using Calabonga.Microservices.Core.QueryParams;
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

        public IActionResult Index(PagedListQueryParams queryParams)
        {
            var operationResult = _constructionObjectsController.GetPaged(queryParams).Value;

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            return View(operationResult);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
