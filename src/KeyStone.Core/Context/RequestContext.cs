namespace KeyStone.Core.Context
{
    public class RequestContext : IRequestContext
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
