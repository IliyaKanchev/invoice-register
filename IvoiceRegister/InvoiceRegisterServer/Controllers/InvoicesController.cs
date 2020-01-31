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
            IEnumerable<Client> items = _context.Clients.Where(x => x.Id == value.ClientId);
            if (!items.Any()) return NotFound(new ApiError("There's no client associated with that invoice."));

            value.Id = 0;
            value.Client = items.First();
            value.ClientId = value.Client.Id;

            _context.Invoices.Add(value);
            _context.SaveChanges();

            return Ok(value);
        }

        // POST api/invoices/update
        [HttpPost("update")]
        public IActionResult Update([FromBody]JObject value)
        {
            var predicate = PredicateBuilder.False<Invoice>();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "id")
                {
                    predicate = predicate.Or(x => x.Id == value.SelectToken(property.Name).Value<int>());
                }
            }

            IEnumerable<Invoice> items = _context.Invoices.Where(predicate);
            if (!items.Any()) return NotFound(new ApiError("There's no invoice associated with that id."));

            Invoice updatable = items.First();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "number")
                {
                   updatable.Number = value.SelectToken(property.Name).Value<int>();
                }

                if (property.Name == "date")
                {
                   updatable.Date = value.SelectToken(property.Name).Value<DateTime>();
                }

                if (property.Name == "description")
                {
                    updatable.Description = value.SelectToken(property.Name).Value<string>();
                }

                if (property.Name == "sum")
                {
                    updatable.Sum = value.SelectToken(property.Name).Value<double>();
                }

                if (property.Name == "client_id")
                {
                   updatable.ClientId = value.SelectToken(property.Name).Value<int>();
                }
            }

            _context.Invoices.Update(updatable);
            _context.SaveChanges();

            return Ok(updatable);
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
