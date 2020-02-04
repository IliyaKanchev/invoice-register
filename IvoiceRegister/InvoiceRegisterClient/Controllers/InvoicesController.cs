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

        // GET: /<invoices>/edit
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            ClientViewModelWithInvoices results = new ClientViewModelWithInvoices();
            results.InvoiceId = id;

            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _invoicesService.List(token, results);

            if (!(status || results.Invoices.Any()))
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            }

            return View("EditInvoice", results.Invoices[0]);
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

        [HttpPost]
        [Route("save", Name = "isave")]
        public IActionResult Save(InvoiceViewModel invoice)
        {
            string token = HttpContext.Session.GetString("InvoiceRegisterJWToken");
            bool status = _invoicesService.Save(token, invoice);

            if (status)
            {
                return Redirect("~/clients/details/" + invoice.ClientId.ToString());
            }

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");

            return View("EditInvoice", invoice);
        }
    }
}
