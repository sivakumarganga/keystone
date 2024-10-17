using System.ComponentModel.DataAnnotations;

namespace KeyStone.Web.Common.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
