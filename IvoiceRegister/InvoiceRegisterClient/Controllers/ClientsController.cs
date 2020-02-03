using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceRegisterClient.Models;
using InvoiceRegisterClient.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterClient.Controllers
{
    [Route("/[controller]")]
    public class ClientsController : Controller
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService authService)
        {
            _clientsService = authService;
        }

        // GET: /<controller>/edit
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            PagedResultViewModel<ClientViewModel> results = new PagedResultViewModel<ClientViewModel>();
            results.Id = id;

            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _clientsService.List(token, results);

            if (!(status || results.Items.Any()))
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View("EditClient", results.Items[0]);
        }

        [HttpPost]
        [Route("save", Name = "save")]
        public IActionResult Save(ClientViewModel client)
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _clientsService.Save(token, client);

            if (status)
            {
                return Redirect("~/");
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");

            return View("EditClient", client);
        }
    }
}
