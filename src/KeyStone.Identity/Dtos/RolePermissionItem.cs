using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyStone.Shared.Models.Identity
{
    public class RolePermissionItem
    {
        public int RoleId { get; set; }
        public List<string> Permissions { get; set; }
    }
}
