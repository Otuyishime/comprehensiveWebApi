using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testWebAPI.DBs;
using testWebAPI.Models.Resources;
using testWebAPI.Models.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/rooms
        [HttpGet(Name = nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            return Ok();
        }

        // GET api/rooms/{roomId}
        [HttpGet("{roomId}", Name = nameof(GetRoomByIdAsync))]
        public async Task<IActionResult> GetRoomByIdAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var room = await _roomService.GetRoomAsync(roomId, cancellationToken);
            return room == null ? NotFound() : (IActionResult)Ok(room);
        }
    }
}
