﻿using KeyStone.Data;
using KeyStone.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KeyStone.Identity.Wrappers.Stores
{
    public class RoleStore : RoleStore<Role, KeyStoneDbContext, int, UserRole, RoleClaim>
    {
        public RoleStore(KeyStoneDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
