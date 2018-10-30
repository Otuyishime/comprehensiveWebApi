using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        private readonly PagingOptions _defaultPagingOptions;

        public RoomsController(IRoomService roomService, IOpeningService openingService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _roomService = roomService;
            _openingService = openingService;
            _defaultPagingOptions = defaultPagingOptions.Value;
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
        public async Task<IActionResult> GetAllRoomOpeningsAsync([FromQuery] PagingOptions pagingOptions, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid){
                return BadRequest(new ApiError(ModelState));
            }

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var openings = await _openingService.GetOpeningsAsync(pagingOptions, cancellationToken);
            var collection = PagedCollection<Opening>.Create(
                Link.ToCollection(nameof(GetAllRoomOpeningsAsync)),
                openings.Items.ToArray(),
                openings.TotalSize,
                pagingOptions
            );

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
