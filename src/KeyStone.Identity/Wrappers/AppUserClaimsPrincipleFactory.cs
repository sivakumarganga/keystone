using KeyStone.Concerns.Domain;
using KeyStone.Identity.Wrappers.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using KeyStone.Data.Models.Identity;

namespace KeyStone.Identity.Wrappers
{
    public class AppUserClaimsPrincipleFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public AppUserClaimsPrincipleFactory(AppUserManager userManager, AppRoleManager roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
        }


        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var userRoles = await UserManager.GetRolesAsync(user);

            var claimsIdentity = await base.GenerateClaimsAsync(user);
            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, user.UserCode));

            foreach (var roles in userRoles)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roles));
            }
            return claimsIdentity;
        }
    }
}
