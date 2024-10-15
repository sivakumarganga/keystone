using KeyStone.Concerns.Identity;
using KeyStone.Shared.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace KeyStone.Core.Contracts.Identity
{
    public interface IRoleManagerService
    {
        Task<List<RoleInfo>> GetRolesAsync();
        Task<IdentityResult> CreateRoleAsync(NewRole model);
        Task<bool> DeleteRoleAsync(int roleId);
        Task<List<PermissionActionDescription>> GetPermissionActionsAsync();
        Task<RolePermission> GetRolePermissionsAsync(int roleId);
        Task<bool> ChangeRolePermissionsAsync(RolePermissionItem model);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<Role> GetRoleByNameAsync(string role);
    }
}
