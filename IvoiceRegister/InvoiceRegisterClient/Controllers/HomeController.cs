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
            PagedResultViewModel<ClientViewModel> model = new PagedResultViewModel<ClientViewModel>();

            if (token == null)
            {
                return Redirect("~/authenticate");
            }

            bool status = _clientsService.List(token, model);

            if (!status)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [Route("/home/filter/", Name = "filter")]
        public IActionResult Filter(PagedResultViewModel<ClientViewModel> model)
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _clientsService.List(token, model);

            if (!status)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View("Index", model);
        }

        [HttpGet]
        [Route("/home/filter/{inc}/{current}/{size}")]
        public IActionResult Filter(int inc, int current, int size)
        {
            PagedResultViewModel<ClientViewModel> model = new PagedResultViewModel<ClientViewModel>();
            model.Page = current + inc;
            model.PageSize = size;

            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _clientsService.List(token, model);

            if (!status)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
