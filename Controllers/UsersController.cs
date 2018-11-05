using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using testWebAPI.Models;
using testWebAPI.Models.Entities;
using testWebAPI.Models.Forms;
using testWebAPI.Models.Resources;
using testWebAPI.Models.Services;

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly PagingOptions _defaultPagingOptions;

        public UsersController(
            IUserService userService,
            IOptions<PagingOptions> defaultPagingOptions)
        {
            _userService = userService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet(Name = nameof(GetVisibleUsersAsync))]
        public async Task<IActionResult> GetVisibleUsersAsync(
            [FromQuery] PagingOptions pagingOptions,
            [FromQuery] SortOptions<User, UserEntity> sortOptions,
            [FromQuery] SearchOptions<User, UserEntity> searchOptions,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            // TODO: Authorization check. Is the user an admin?

            // TODO: Return a collection of visible users
            var users = await _userService.GetUsersAsync(
                pagingOptions, sortOptions, searchOptions, cancellationToken);

            var collection = PagedCollection<User>.Create(
                Link.To(nameof(GetVisibleUsersAsync)),
                users.Items.ToArray(),
                users.TotalSize,
                pagingOptions);

            return Ok(collection);
        }

        // Note: The Authorize Attribute is poorly named! It should be Authenticate
        // Authorize attribute tells AspNetCore to only let authenticated requests to access a route
        [Authorize(AuthenticationSchemes = "Bearer")] // specify the authentication scheme
        [HttpGet("me", Name = nameof(GetMeAsync))]
        public async Task<IActionResult> GetMeAsync(CancellationToken cancellationToken)
        {
            if (User == null) return BadRequest();

            var user = await _userService.GetUserAsync(User);
            return user == null ? NotFound() : (IActionResult)Ok(user);
        }

        [HttpPost(Name = nameof(RegisterUserAsync))]
        public async Task<IActionResult> RegisterUserAsync(
            [FromBody] RegisterForm form,
            CancellationToken cancellationToken
        )
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));

            var (succeeded, error) = await _userService.CreateUserAsync(form);
            return succeeded 
                ? Created(Url.Link(nameof(GetMeAsync), null), null)
                    : (IActionResult)BadRequest(new ApiError 
                        { 
                            Message = "Registration failed.", 
                            Detail = error 
                        });
        }

        [Authorize]
        [HttpGet("{userId}", Name = nameof(GetUserByIdAsync))]
        public Task<IActionResult> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            // TODO is userId the current user's ID?
            // If so, return myself.
            // If not, only Admin roles should be able to view arbitrary users.
            throw new NotImplementedException();
        }
    }
}
