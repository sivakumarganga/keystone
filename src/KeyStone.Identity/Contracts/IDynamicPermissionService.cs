using System.Security.Claims;

namespace KeyStone.Identity.Contracts
{
    public interface IDynamicPermissionService
    {
        bool CanAccess(ClaimsPrincipal user, string area, string controller, string action);
    }
}
