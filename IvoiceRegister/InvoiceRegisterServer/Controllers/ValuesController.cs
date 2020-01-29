using Microsoft.AspNetCore.Mvc;
using InvoiceRegisterServer.Code;

namespace InvoiceRegisterServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // POST api/values
        [HttpPost]
        public TestModel Post([FromBody]TestModel value)
        {
            return value;
        }

        // POST api/values/update
        [HttpPost("update")]
        public TestModel PostUpdate([FromBody]TestModel value)
        {
            value.Text += " new";
            return value;
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
