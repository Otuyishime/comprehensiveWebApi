using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using testWebAPI.Models.Entities;

namespace testWebAPI.DBs
{
    public class HotelApiContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public HotelApiContext(DbContextOptions dbContextOptions) : base(dbContextOptions){}

        // Add all the dbSets/Tables that wil be tracked by the context
        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
