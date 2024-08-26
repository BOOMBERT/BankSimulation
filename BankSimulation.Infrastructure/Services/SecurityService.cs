namespace BankSimulation.Infrastructure.Services
{
    internal static class SecurityService
    {
        public static string HashText(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public static bool VerifyHashedText(string plainText, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hashedText);
        }
    }
}
