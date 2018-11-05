using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Forms;
using testWebAPI.Models.Resources;

namespace testWebAPI.Models.Services
{
    public class DefaultUserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;

        public DefaultUserService(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool Succeeded, string Error)> CreateUserAsync(RegisterForm form)
        {
            var entity = new UserEntity
            {
                Email = form.Email,
                UserName = form.Email,  // if you want users to have usernames different from email, implement that here
                FirstName = form.FirstName,
                LastName = form.LastName,
                CreatedAt = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(entity, form.Password);
            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description;
                return (false, firstError);
            }

            return (true, null);
        }

        public async Task<PagedResults<User>> GetUsersAsync(
            PagingOptions pagingOptions,
            SortOptions<User, UserEntity> sortOptions,
            SearchOptions<User, UserEntity> searchOptions,
            CancellationToken cancellationToken)
        {
            IQueryable<UserEntity> query = _userManager.Users;
            query = searchOptions.Apply(query);
            query = sortOptions.Apply(query);

            var size = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<User>()
                .ToArrayAsync(cancellationToken);

            return new PagedResults<User>
            {
                Items = items,
                TotalSize = size
            };
        }
    }
}
