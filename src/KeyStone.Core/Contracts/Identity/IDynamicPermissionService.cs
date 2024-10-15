using System.Security.Claims;

namespace KeyStone.Core.Contracts.Identity
{
    public interface IDynamicPermissionService
    {
        bool CanAccess(ClaimsPrincipal user, string area, string controller, string action);
    }
}
