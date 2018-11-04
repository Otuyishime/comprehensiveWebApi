using System;
using Microsoft.AspNetCore.Identity;

namespace testWebAPI.Models.Entities
{
    public class UserRoleEntity : IdentityRole<Guid>
    {
        public UserRoleEntity()
            : base()
        { }

        public UserRoleEntity(string roleName)
            : base(roleName)
        { }
    }
}
