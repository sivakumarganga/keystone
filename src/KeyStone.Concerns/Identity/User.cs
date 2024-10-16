﻿using KeyStone.Concerns.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Concerns.Domain
{
    public class User : IdentityUser<int>
    {
        public User()
        {
            this.UserCode = Guid.NewGuid().ToString().Substring(0, 8);
        }

        public string Name { get; set; } = String.Empty;
        public string FamilyName { get; set; } = String.Empty;
        public string UserCode { get; set; } = String.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = null!;
        public ICollection<UserLogin> Logins { get; set; } = null!;
        public ICollection<UserClaim> Claims { get; set; } = null!;
        public ICollection<UserToken> Tokens { get; set; } = null!;
        public ICollection<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
    }
}
