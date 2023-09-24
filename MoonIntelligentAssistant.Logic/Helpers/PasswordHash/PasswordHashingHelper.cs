namespace MoonIntelligentAssistant.Logic;

internal static class PasswordHashingHelper
{
    #region Constants
    private const int KeySize = 64;
    private const int Iterations = 350000;
    #endregion

    #region Properties
    private static HashAlgorithmName HashAlgorithm => HashAlgorithmName.SHA512;
    #endregion

    #region Internals
    internal static PasswordHasherDto EncryptPassword(string strPassword)
    {
        byte[] baSalt = RandomNumberGenerator.GetBytes(KeySize);
        byte[] baHash = Rfc2898DeriveBytes.Pbkdf2(
                                                  Encoding.UTF8.GetBytes(strPassword),
                                                  baSalt,
                                                  Iterations,
                                                  HashAlgorithm,
                                                  KeySize);

        PasswordHasherDto hasherDto = new()
                                      {
                                          PasswordSalt = baSalt,
                                          PasswordHash = Convert.ToHexString(baHash)
                                      };

        return hasherDto;
    }

    internal static bool VerifyHashedPassword(string strPassword, PasswordHasherDto hasherDto)
    {
        byte[] hashToCompare = Rfc2898DeriveBytes.Pbkdf2(strPassword, hasherDto.PasswordSalt, Iterations, HashAlgorithm, KeySize);

        return hashToCompare.SequenceEqual(Convert.FromHexString(hasherDto.PasswordHash));
    }
    #endregion
}