using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterClient.Models;
using Microsoft.AspNetCore.Http;

namespace InvoiceRegisterClient.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");

            if (token == null)
            {
                return Redirect("~/authenticate");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
