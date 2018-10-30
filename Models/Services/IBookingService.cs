using System;
using System.Threading;
using System.Threading.Tasks;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public interface IBookingService
    {
        Task<Booking> GetBookingAsync(Guid bookingId, CancellationToken ct);

        Task<Guid> CreateBookingAsync(
            Guid userId,
            Guid roomId,
            DateTimeOffset startAt,
            DateTimeOffset endAt,
            CancellationToken ct);
    }
}
