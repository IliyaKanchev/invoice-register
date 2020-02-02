using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterClient.Models;
using Microsoft.AspNetCore.Http;
using InvoiceRegisterClient.Services;

namespace InvoiceRegisterClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClientsService _clientsService;

        public HomeController(IClientsService authService)
        {
            _clientsService = authService;
        }

        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");

            if (token == null)
            {
                return Redirect("~/authenticate");
            }

            PagedResultViewModel<ClientViewModel> list = _clientsService.List(token);

            if (list == null)
            {
                list = new PagedResultViewModel<ClientViewModel>();
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(list);
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
