using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using testWebAPI.DBs;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly HotelApiContext _hotelApiContext;

        public DefaultRoomService(HotelApiContext hotelApiContext)
        {
            _hotelApiContext = hotelApiContext;
        }

        public async Task<Room> GetRoomAsync(Guid roomId, CancellationToken cancellationToken)
        {
            var entity = await _hotelApiContext.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, cancellationToken);
            return entity == null ? null : Mapper.Map<Room>(entity);
        }

        public async Task<PagedResults<Room>> GetRoomsAsync(
            PagingOptions pagingOptions,
            SortOptions<Room, RoomEntity> sortOptions,
            SearchOptions<Room, RoomEntity> searchOptions,
            CancellationToken cancellationToken)
        {
            IQueryable<RoomEntity> query = _hotelApiContext.Rooms;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<Room>()
                .ToArrayAsync(cancellationToken);

            return new PagedResults<Room>
            {
                Items = items,
                TotalSize = size
            };
        }
    }
}
