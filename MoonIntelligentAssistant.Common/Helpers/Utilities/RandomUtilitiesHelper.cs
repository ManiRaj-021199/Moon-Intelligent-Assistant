namespace MoonIntelligentAssistant.Common;

public class RandomUtilitiesHelper
{
    #region Publics
    public static string GenerateRandomString()
    {
        byte[] randomNumber = new byte[8];

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }
    #endregion
}