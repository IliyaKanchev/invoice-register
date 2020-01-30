using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterServer.Code;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterServer.Controllers
{
    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly DbServiceContext _context;

        public InvoicesController(DbServiceContext context)
        {
            _context = context;
        }

        // POST api/invoices/list
        [HttpPost("list")]
        public IEnumerable<Invoice> List([FromBody]JObject value)
        {
            var predicate = PredicateBuilder.True<Invoice>();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "number")
                {
                    predicate = predicate.And(x => x.Number == value.SelectToken(property.Name).Value<int>());
                }

                if (property.Name == "id")
                {
                    predicate = predicate.And(x => x.Id == value.SelectToken(property.Name).Value<int>());
                }

                if (property.Name == "date")
                {
                    predicate = predicate.And(x => x.Date == value.SelectToken(property.Name).Value<DateTime>());
                }

                if (property.Name == "description")
                {
                    predicate = predicate.And(x => x.Description == value.SelectToken(property.Name).Value<string>());
                }

                if (property.Name == "sum")
                {
                    predicate = predicate.And(x => x.Sum.Equals(value.SelectToken(property.Name).Value<double>()));
                }

                if (property.Name == "client_id")
                {
                    predicate = predicate.And(x => x.ClientId == value.SelectToken(property.Name).Value<int>());
                }
            }

            return _context.Invoices.Where(predicate);
        }

        // POST api/invoices/insert
        [HttpPost("insert")]
        public IActionResult Insert([FromBody]Invoice value)
        {
            IEnumerable<Client> clients = _context.Clients.Where(x => x.Id == value.ClientId);
            if (!clients.Any()) return NotFound("There's no client associated with that invoice.");

            value.Id = 0;
            value.Client = clients.First();
            value.ClientId = value.Client.Id;

            _context.Invoices.Add(value);
            _context.SaveChanges();

            return Ok(value);
        }

        // POST api/invoices/update
        [HttpPost("update")]
        public TestModel Update([FromBody]TestModel value)
        {
            value.Text += " update";
            return value;
        }

        // POST api/invoices/delete
        [HttpPost("delete")]
        public TestModel Delete([FromBody]TestModel value)
        {
            value.Text += " delete";
            return value;
        }
    }
}
