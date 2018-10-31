using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomAsync(Guid roomId, CancellationToken cancellationToken);

        Task<PagedResults<Room>> GetRoomsAsync(
            PagingOptions pagingOptions, 
            SortOptions<Room, RoomEntity> sortOptions,
            CancellationToken cancellationToken
        );
    }
}
