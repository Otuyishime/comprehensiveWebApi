using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using testWebAPI.DBs;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public class DefaultBookingService : IBookingService
    {
        private readonly HotelApiContext _context;
        private readonly IDateLogicService _dateLogicService;

        public DefaultBookingService(
            HotelApiContext context,
            IDateLogicService dateLogicService)
        {
            _context = context;
            _dateLogicService = dateLogicService;
        }

        public async Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt,
            CancellationToken cancellationToken)
        {
            var room = await _context.Rooms
                .SingleOrDefaultAsync(r => r.Id == roomId, cancellationToken);
            if (room == null) throw new ArgumentException("Invalid room id.");

            var minimumStay = _dateLogicService.GetMinimumStay();
            // Might be a good idea to move this logic to a class that calculates prices and rates
            var total = (int)((endAt - startAt).TotalHours / minimumStay.TotalHours) * room.Rate;

            var id = Guid.NewGuid();

            var newBooking = _context.Bookings.Add(new BookingEntity
            {
                Id = id,
                CreatedAt = DateTimeOffset.UtcNow,
                ModifiedAt = DateTimeOffset.UtcNow,
                StartAt = startAt.ToUniversalTime(),
                EndAt = endAt.ToUniversalTime(),
                Room = room,
                Total = total
            });

            var created = await _context.SaveChangesAsync(cancellationToken);
            if (created < 1) throw new InvalidOperationException("Could not create the booking.");

            return id;
        }

        public async Task<Booking> GetBookingAsync(
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            var entity = await _context.Bookings
                .SingleOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

            return entity == null ? null : Mapper.Map<Booking>(entity);
        }
    }
}
