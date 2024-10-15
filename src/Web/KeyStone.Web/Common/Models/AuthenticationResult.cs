namespace KeyStone.Web.Common.Models
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; set; }
        public string[] Errors { get; set; } = [];
        public int ExpiresIn { get; set; }
        public static AuthenticationResult Success(LoginResult loginResult)
        {
            return new AuthenticationResult
            {
                IsAuthenticated = true,
                ExpiresIn = loginResult.ExpiresIn,
            };
        }
        public static AuthenticationResult Fail(params string[] errors)
        {
            return new AuthenticationResult
            {
                IsAuthenticated = false,
                Errors = errors
            };
        }
    }
}
