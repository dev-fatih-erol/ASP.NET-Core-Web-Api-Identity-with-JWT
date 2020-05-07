namespace Users.Api.Infrastructure.Helpers
{
    public class UrlBuilder
    {
        private const string ApplicationUrl = "https://localhost:5001";

        public static string EmailConfirmationLink(int userId, string code)
        {
            return $"{ApplicationUrl}/Account/ConfirmEmail?userId={userId}&code={code}";
        }

        public static string ResetPasswordCallbackLink(int userId, string code)
        {
            return $"{ApplicationUrl}/Account/ResetPassword?userId={userId}&code={code}";
        }
    }
}