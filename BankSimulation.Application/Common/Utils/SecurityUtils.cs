namespace BankSimulation.Application.Common.Utils
{
    internal static class SecurityUtils
    {
        internal static string HashText(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        internal static bool VerifyHashedText(string plainText, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hashedText);
        }
    }
}
