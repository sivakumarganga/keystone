namespace KeyStone.Web.Common.Models
{
    public class UserContext
    {
        public int UserId { get; set; }
        public string UserKey { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsAdmin { get => !string.IsNullOrEmpty(Role) && Role.Equals(Constants.Roles.Admin); }
        public string AgencyName { get; set; } = string.Empty;
    }
}
