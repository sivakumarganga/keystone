using KeyStone.Concerns.Identity;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Identity.Wrappers.Validators
{
    public class AppRoleValidator : RoleValidator<Role>
    {
        public AppRoleValidator(IdentityErrorDescriber errors) : base(errors)
        {

        }

        public override Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
        {
            return base.ValidateAsync(manager, role);
        }
    }
}
