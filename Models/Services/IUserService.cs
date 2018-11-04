using System;
using System.Threading;
using System.Threading.Tasks;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public interface IUserService
    {
        Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            CancellationToken cancellationToken);
    }
}
