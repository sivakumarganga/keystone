using KeyStone.Concerns.Domain;

namespace KeyStone.Concerns.Identity
{
    public class UserRefreshToken
    {
        public UserRefreshToken()
        {
            CreatedAt = DateTime.Now;
        }
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsValid { get; set; }
    }
}
