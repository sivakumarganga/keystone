namespace KeyStone.Core.Context
{
    public interface IRequestContext
    {
        int UserId { get; set; }
        string UserName { get; set; }
        string DisplayName { get; set; }
        string Role { get; set; }
        bool IsAdmin() => Role.ToUpper() == "ADMIN";

    }
}
