using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterServer.Code;
using Newtonsoft.Json.Linq;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterServer.Controllers
{
    [Authorize]
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
        public PagedResult<Invoice> List([FromBody]JObject value)
        {
            bool ascending = false;

            int page = 0;
            int pageSize = 0;
            int pagesCount = 0;

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

                if (property.Name == "before")
                {
                    predicate = predicate.And(x => x.Date <= value.SelectToken(property.Name).Value<DateTime>());
                }

                if (property.Name == "after")
                {
                    predicate = predicate.And(x => x.Date >= value.SelectToken(property.Name).Value<DateTime>());
                }

                if (property.Name == "reversed")
                {
                    ascending = value.SelectToken(property.Name).Value<bool>();
                }

                if (property.Name == "page")
                {
                    page = value.SelectToken(property.Name).Value<int>();
                }

                if (property.Name == "page_size")
                {
                    pageSize = value.SelectToken(property.Name).Value<int>();
                }
            }

            if (page > 0 && pageSize > 0)
            {
                pagesCount = _context.Invoices.Count() / pageSize;

                if (ascending) return new PagedResult<Invoice>(page, pageSize, pagesCount, _context.Invoices.Where(predicate).OrderBy(x => x.Id).ToPagedList(page, pageSize).ToList());
                else return new PagedResult<Invoice>(page, pageSize, pagesCount, _context.Invoices.Where(predicate).OrderByDescending(x => x.Id).ToPagedList(page, pageSize).ToList());
            }
            else
            {
                if (ascending) return new PagedResult<Invoice>(page, pageSize, pagesCount, _context.Invoices.Where(predicate).OrderBy(x => x.Id).ToList());
                else return new PagedResult<Invoice>(page, pageSize, pagesCount, _context.Invoices.Where(predicate).OrderByDescending(x => x.Id).ToList());
            }

        }

        // POST api/invoices/insert
        [HttpPost("insert")]
        public IActionResult Insert([FromBody]Invoice value)
        {
            // Maybe remove this?
            IEnumerable<Invoice> existing = _context.Invoices.Where(x => x.Number == value.Number);
            if (existing.Any()) return NotFound(new ApiError("There's already a record for an invoice with this number."));

            value.Id = 0;

            // Maybe remove this?
            IEnumerable<Client> items = _context.Clients.Where(x => x.Id == value.ClientId);
            if (!items.Any()) return NotFound(new ApiError("There's no client associated with that invoice."));

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

            Invoice found = items.First();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "number")
                {
                   found.Number = value.SelectToken(property.Name).Value<int>();
                }

                if (property.Name == "date")
                {
                   found.Date = value.SelectToken(property.Name).Value<DateTime>();
                }

                if (property.Name == "description")
                {
                    found.Description = value.SelectToken(property.Name).Value<string>();
                }

                if (property.Name == "sum")
                {
                    found.Sum = value.SelectToken(property.Name).Value<double>();
                }

                if (property.Name == "client_id")
                {
                   found.ClientId = value.SelectToken(property.Name).Value<int>();
                }
            }

            _context.Invoices.Update(found);
            _context.SaveChanges();

            return Ok(found);
        }

        // POST api/invoices/delete
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]JObject value)
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

            Invoice found = items.First();

            _context.Invoices.Remove(found);
            _context.SaveChanges();

            return Ok(found);
        }
    }
}
