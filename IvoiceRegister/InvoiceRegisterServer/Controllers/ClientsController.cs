﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterServer.Code;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InvoiceRegisterServer.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        // POST api/clients/list
        [HttpPost("list")]
        public TestModel List([FromBody]TestModel value)
        {
            value.Text += " list";
            return value;
        }

        // POST api/clients/insert
        [HttpPost("insert")]
        public TestModel Insert([FromBody]TestModel value)
        {
            value.Text += " insert";
            return value;
        }

        // POST api/clients/update
        [HttpPost("update")]
        public TestModel Update([FromBody]TestModel value)
        {
            value.Text += " update";
            return value;
        }

        // POST api/clients/delete
        [HttpPost("delete")]
        public TestModel Delete([FromBody]TestModel value)
        {
            value.Text += " delete";
            return value;
        }
    }
}