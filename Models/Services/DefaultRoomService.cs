using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using testWebAPI.DBs;
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
    }
}
