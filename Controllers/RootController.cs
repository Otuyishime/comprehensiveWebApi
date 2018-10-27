using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// ****************************
//     THE ROUTE OF THE API
// ****************************
namespace testWebAPI.Controllers
{
    [Route("/api/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    {
        // GET: api/values
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new
            {
                href = Url.Link(nameof(GetRoot), null),
                info = new { href = Url.Link(nameof(InfoController.GetInfo), null) }
            };

            return Ok(response);
        }
    }
}
