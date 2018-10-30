using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testWebAPI.DBs;
using testWebAPI.Models;
using testWebAPI.Models.Resources;
using testWebAPI.Models.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IOpeningService _openingService;

        public RoomsController(IRoomService roomService, IOpeningService openingService)
        {
            _roomService = roomService;
            _openingService = openingService;
        }

        // GET: api/rooms
        [HttpGet(Name = nameof(GetRoomsAsync))]
        public async Task<IActionResult> GetRoomsAsync(CancellationToken cancellationToken)
        {
            var rooms = await _roomService.GetRoomsAsync(cancellationToken);

            var collectionLink = Link.ToCollection(nameof(GetRoomsAsync));
            var collection = new Collection<Room> {
                Self = collectionLink,
                Value = rooms.ToArray()
            };

            return Ok(collection);
        }

        // GET /rooms/openings
        [HttpGet("openings", Name = nameof(GetAllRoomOpeningsAsync))]
        public async Task<IActionResult> GetAllRoomOpeningsAsync(CancellationToken cancellationToken)
        {
            var openings = await _openingService.GetOpeningsAsync(cancellationToken);

            var collection = new Collection<Opening>()
            {
                Self = Link.ToCollection(nameof(GetAllRoomOpeningsAsync)),
                Value = openings.ToArray()
            };

            return Ok(collection);
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
