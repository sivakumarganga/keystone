namespace KeyStone.Web.Common
{
    public class Constants
    {
        public const string ApiHttpClient = "api";
        public static string RootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
        public const string TokenLocalStorageKey = "auth";
        public const string UserContextLocalStorageKey = "userContext";
        public const string Info = "Info";
        public const string Warning = "Warning";
        public const string Error = "Error";
        public const string Success = "Success";
        public const string SomethingWentWrong = "Something went wrong";
        public static class Roles
        {
            public const string Admin = "admin";
            public const string Agent = "agent";
        }
    }

    public static class Endpoint
    {
        public const string Token = "Connect/Token";
        public const string Register = "Connect/Register";
        public const string RefreshSignIn = "Connect/RefreshSignIn";
        public const string UserInfo = "Connect/UserInfo";
        public const string Logout = "Connect/Logout";
    }
}
