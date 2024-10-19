namespace KeyStone.Shared.Models;

public class UserInfo
{
    public int UserId { get; set;}
    public string UserKey { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public IReadOnlyCollection<string> Roles { get; set; } = [];
}