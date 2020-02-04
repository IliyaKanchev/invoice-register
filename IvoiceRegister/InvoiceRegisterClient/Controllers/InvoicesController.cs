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
    public class InvoicesController : Controller
    {
        private readonly IInvoicesService _invoicesService;

        public InvoicesController(IInvoicesService authService)
        {
            _invoicesService = authService;
        }

        [HttpPost]
        [Route("filter", Name = "ifilter")]
        public IActionResult Filter(ClientViewModelWithInvoices model)
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _invoicesService.List(token, model);

            if (!status)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View("DetailedClient", model);
        }
    }
}
