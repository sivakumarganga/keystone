﻿using KeyStone.Concerns.Domain;
using KeyStone.Data.Models.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KeyStone.Identity.Wrappers.Managers
{
    public class AppSignInManager : SignInManager<User>
    {
        public AppSignInManager(UserManager<User> userManager,
                                IHttpContextAccessor contextAccessor,
                                IUserClaimsPrincipalFactory<User> claimsFactory,
                                IOptions<IdentityOptions> optionsAccessor,
                                ILogger<SignInManager<User>> logger,
                                IAuthenticationSchemeProvider schemes,
                                IUserConfirmation<User> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
