﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public interface IOpeningService
    {
        Task<PagedResults<Opening>> GetOpeningsAsync(PagingOptions pagingOptions, CancellationToken ct);

        Task<IEnumerable<BookingRange>> GetConflictingSlots(
            Guid roomId,
            DateTimeOffset start,
            DateTimeOffset end,
            CancellationToken ct);
    }
}