namespace MoonIntelligentAssistant.Common;

public static class StringExtensionHelper
{
    #region Publics
    public static bool IsValidEmail(this string strEmail)
    {
        if(strEmail.EndsWith(".")) return false;

        try
        {
            MailAddress mail = new(strEmail);
            return mail.Address == strEmail;
        }
        catch
        {
            return false;
        }
    }
    #endregion
}