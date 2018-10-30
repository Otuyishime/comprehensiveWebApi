using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testWebAPI.Models.Services;

namespace testWebAPI.Controllers
{
    [Route("/[controller]")]
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // TODO: authorization
        [HttpGet("{bookingId}", Name = nameof(GetBookingByIdAsync))]
        public async Task<IActionResult> GetBookingByIdAsync(
            Guid bookingId,
            CancellationToken ct)
        {
            var booking = await _bookingService.GetBookingAsync(bookingId, ct);
            return booking == null ? NotFound() : (IActionResult)Ok(booking);
        }
    }
}