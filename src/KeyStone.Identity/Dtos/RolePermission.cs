
using KeyStone.Data.Models.Identity;
using KeyStone.Shared.Models.Identity;

namespace KeyStone.Identity.Dtos
{
    public class RolePermission
    {
        public List<string> Keys { get; set; } = new List<string>();

        public Role Role { get; set; }

        public int RoleId { get; set; }

        public List<PermissionActionDescription> Actions { get; set; }
    }
}
