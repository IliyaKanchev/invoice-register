﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterServer.Code;
using Newtonsoft.Json.Linq;
using X.PagedList;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterServer.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        private readonly DbServiceContext _context;

        public ClientsController(DbServiceContext context)
        {
            _context = context;
        }

        // POST api/clients/list
        [HttpPost("list")]
        public PagedResult<Client> List([FromBody]JObject value)
        {
            bool ascending = false;

            int page = 0;
            int pageSize = 0;
            int pagesCount = 0;

            var predicate = PredicateBuilder.True<Client>();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "id")
                {
                    predicate = predicate.And(x => x.Id == value.SelectToken(property.Name).Value<int>());
                }

                if (property.Name == "name")
                {
                    predicate = predicate.And(x => x.Name == value.SelectToken(property.Name).Value<string>());
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
                pagesCount = _context.Clients.Count() / pageSize;

                if (ascending) return new PagedResult<Client>(page, pageSize, pagesCount, _context.Clients.Include(client => client.Invoices).Where(predicate).OrderBy(x => x.Id).ToPagedList(page, pageSize).ToList());
                else return new PagedResult<Client>(page, pageSize, pagesCount, _context.Clients.Include(client => client.Invoices).Where(predicate).OrderByDescending(x => x.Id).ToPagedList(page, pageSize).ToList());
            }
            else
            {
                if (ascending) return new PagedResult<Client>(page, pageSize, pagesCount, _context.Clients.Include(client => client.Invoices).Where(predicate).OrderBy(x => x.Id).ToList());
                else return new PagedResult<Client>(page, pageSize, pagesCount, _context.Clients.Include(client => client.Invoices).Where(predicate).OrderByDescending(x => x.Id).ToList());
            }
        }

        // POST api/clients/insert
        [HttpPost("insert")]
        public IActionResult Insert([FromBody]Client value)
        {
            value.Id = 0;

            _context.Clients.Add(value);
            _context.SaveChanges();

            return Ok(value);
        }

        // POST api/clients/update
        [HttpPost("update")]
        public IActionResult Update([FromBody]JObject value)
        {
            var predicate = PredicateBuilder.False<Client>();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "id")
                {
                    predicate = predicate.Or(x => x.Id == value.SelectToken(property.Name).Value<int>());
                }
            }

            IEnumerable<Client> items = _context.Clients.Include(client => client.Invoices).Where(predicate);
            if (!items.Any()) return NotFound(new ApiError("There's no client associated with that id."));

            Client found = items.First();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "name")
                {
                    found.Name = value.SelectToken(property.Name).Value<string>();
                }
            }

            _context.Clients.Update(found);
            _context.SaveChanges();

            return Ok(found);
        }

        // POST api/clients/delete
        [HttpPost("delete")]
        public IActionResult Delete([FromBody]JObject value)
        {
            var predicate = PredicateBuilder.False<Client>();

            foreach (JProperty property in value.Properties())
            {
                if (property.Name == "id")
                {
                    predicate = predicate.Or(x => x.Id == value.SelectToken(property.Name).Value<int>());
                }
            }

            IEnumerable<Client> items = _context.Clients.Include(client => client.Invoices).Where(predicate);
            if (!items.Any()) return NotFound(new ApiError("There's no client associated with that id."));

            Client found = items.First();

            _context.Clients.Remove(found);
            _context.SaveChanges();

            return Ok(found);
        }
    }
}
