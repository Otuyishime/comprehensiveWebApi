using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        // GET: api/tests
        [HttpGet]
        public IActionResult Get() => throw new NotImplementedException();

        // GET api/tests/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/tests
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tests/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tests/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
