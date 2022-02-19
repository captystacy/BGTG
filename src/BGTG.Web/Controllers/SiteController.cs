﻿using System.Diagnostics;
using BGTG.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BGTG.Web.Controllers
{
    public class SiteController : Controller
    {
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
