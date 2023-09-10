namespace MoonIntelligentAssistant.Common;

public static class EmailHelper
{
    #region Publics
    public static void SendEmail(MailRequestDto dtoMailRequest, Dictionary<string, string> dictArguments)
    {
        SmtpClient smtpClient = new(MailConstantValues.ClientSMTP)
                                {
                                    Port = 587,
                                    Credentials = new NetworkCredential(MailCredentials.Address, MailCredentials.Password),
                                    EnableSsl = true
                                };

        MailMessage message = new()
                              {
                                  From = new MailAddress(MailConstantValues.MailNoReply),
                                  To = { dtoMailRequest.ToMailAddress },
                                  IsBodyHtml = true
                              };

        switch(dtoMailRequest.MailType)
        {
            case MailType.UserRegisterAuthCode:
                string strBodyContent = File.ReadAllText(Path.GetFullPath(dtoMailRequest.MailTemplatePath));
                strBodyContent = strBodyContent.Replace("UserNameReplace", dictArguments["UserName"]);
                strBodyContent = strBodyContent.Replace("AuthenticationCodeReplace", dictArguments["AuthenticationCode"]);

                message.Subject = MailSubjectValues.UserRegisterAuthCode;
                message.Body = strBodyContent;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        smtpClient.Send(message);
    }
    #endregion
}