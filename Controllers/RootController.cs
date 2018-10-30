using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testWebAPI.Models;
using testWebAPI.Models.Resources;

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
            var response = new RootResponse
            {
                Self = Link.To(nameof(this.GetRoot)),
                Rooms = Link.To(nameof(RoomsController.GetRoomsAsync)),
                Info = Link.To(nameof(InfoController.GetInfo))
            };

            return Ok(response);
        }
    }
}
