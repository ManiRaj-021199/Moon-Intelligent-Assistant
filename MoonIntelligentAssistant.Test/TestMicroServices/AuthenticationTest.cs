namespace MoonIntelligentAssistant.Test;

[TestClass]
public class AuthenticationTest : MoonIATestBase
{
    #region Fields
    private AuthenticationFacade facadeAuthentication = null!;
    #endregion

    #region Initialize and Cleanup
    [TestInitialize]
    public void TestInitialize()
    {
        facadeAuthentication = new AuthenticationFacade(entityLog, entityUsers, entityUserAuthentication, dbContextCommonActivities);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [User].Users");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [User].UserAuthentication");

        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [Log].Info");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [Log].Error");
    }
    #endregion

    #region Tests
    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_Success()
    {
        // Act
        BaseApiResponseDto dtoResponse = await SendUserRegisterAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationSuccessMessages.SendAuthenticationCode, dtoResponse.ResponseMessage);

        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreEqual(AuthenticationTestConst.TEMP_NAME, dtoUserAuthentication.UserName);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoUserAuthentication.UserEmail);
        Assert.IsFalse(dtoUserAuthentication.IsAuthenticated);
    }

    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_InvalidEmail()
    {
        // Arrange
        UserRegisterDto dtoUserRegister = new()
                                          {
                                              UserEmail = "invalid email",
                                              UserName = AuthenticationTestConst.TEMP_NAME
                                          };

        // Act
        BaseApiResponseDto dtoResponse = await facadeAuthentication.SendUserRegisterAuthCode(dtoUserRegister);

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.InvalidEmail, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_UserAlreadyRegistered()
    {
        // Arrange
        await SendUserRegisterAuthCode();
        await ValidateAuthCode();
        await RegisterPassword();

        // Act
        BaseApiResponseDto dtoResponse = await SendUserRegisterAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserAlreadyRegistered, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_UserIsAlreadyInNonAuth_UpdateUserAuthCode()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoUserAuthentication.UserEmail);

        DateTime dtFirstUpdate = dtoUserAuthentication.RegisterDate;

        // Act
        await SendUserRegisterAuthCode();

        // Assert
        dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoUserAuthentication.UserEmail);
        Assert.AreNotEqual(dtFirstUpdate, dtoUserAuthentication.RegisterDate);
    }

    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_AuthCodeExpired()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        Assert.IsNotNull(dtoUserAuthentication);

        string strExpiredAuthCode = dtoUserAuthentication.AuthCode;

        dtoUserAuthentication.RegisterDate -= new TimeSpan(0, 5, 1);

        entityUserAuthentication.NonAuthUsersWriter.Update(dtoUserAuthentication);
        await dbContextCommonActivities.PersistAsync();

        // Act
        BaseApiResponseDto dtoResponse = await SendUserRegisterAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationSuccessMessages.SendAuthenticationCode, dtoResponse.ResponseMessage);

        dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreNotEqual(strExpiredAuthCode, dtoUserAuthentication.AuthCode);
    }

    [TestMethod]
    public async Task Test_ValidateAuthCode_Success()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        // Act
        BaseApiResponseDto dtoResponse = await ValidateAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationSuccessMessages.AuthCodeValidateSuccess, dtoResponse.ResponseMessage);

        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoUserAuthentication.UserEmail);
        Assert.IsTrue(dtoUserAuthentication.IsAuthenticated);
    }

    [TestMethod]
    public async Task Test_ValidateAuthCode_UserAlreadyRegistered()
    {
        // Arrange
        await SendUserRegisterAuthCode();
        await ValidateAuthCode();
        await RegisterPassword();

        // Act
        BaseApiResponseDto dtoResponse = await ValidateAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserAlreadyRegistered, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_ValidateAuthCode_UserNotRegistered()
    {
        // Arrange

        // Act
        BaseApiResponseDto dtoResponse = await ValidateAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserNotRegistered, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_ValidateAuthCode_WrongAuthCode()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        // Act
        ValidateAuthCodeDto dtoValidateAuthCode = new()
                                                  {
                                                      UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                                      AuthenticationCode = "wrong auth code"
                                                  };

        BaseApiResponseDto dtoResponse = await facadeAuthentication.ValidateAuthCode(dtoValidateAuthCode);

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.WrongAuthCode, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_ValidateAuthCode_AuthCodeExpired()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        Assert.IsNotNull(dtoUserAuthentication);

        string strExpiredAuthCode = dtoUserAuthentication.AuthCode;

        dtoUserAuthentication.RegisterDate -= new TimeSpan(0, 5, 1);

        entityUserAuthentication.NonAuthUsersWriter.Update(dtoUserAuthentication);
        await dbContextCommonActivities.PersistAsync();

        // Act
        BaseApiResponseDto dtoResponse = await ValidateAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.ExpiredAuthCode, dtoResponse.ResponseMessage);

        dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUserAuthentication);
        Assert.AreNotEqual(strExpiredAuthCode, dtoUserAuthentication.AuthCode);
    }

    [TestMethod]
    public async Task Test_RegisterPassword_Success()
    {
        // Arrange
        await SendUserRegisterAuthCode();
        await ValidateAuthCode();

        // Act
        BaseApiResponseDto dtoResponse = await RegisterPassword();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationSuccessMessages.UserRegisterSuccess, dtoResponse.ResponseMessage);

        UserDto? dtoUser = entityUsers.AuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoUser);
        Assert.IsNull(dtoUserAuthentication);

        Assert.AreEqual(AuthenticationTestConst.TEMP_NAME, dtoUser.UserName);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoUser.UserEmail);
    }

    [TestMethod]
    public async Task Test_RegisterPassword_UserAlreadyRegistered()
    {
        // Arrange
        await SendUserRegisterAuthCode();
        await ValidateAuthCode();
        await RegisterPassword();

        // Act
        BaseApiResponseDto dtoResponse = await RegisterPassword();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserAlreadyRegistered, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_RegisterPassword_UserNotRegistered()
    {
        // Arrange

        // Act
        BaseApiResponseDto dtoResponse = await RegisterPassword();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserNotRegistered, dtoResponse.ResponseMessage);
    }

    [TestMethod]
    public async Task Test_RegisterPassword_UserNotAuthenticated()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        // Act
        BaseApiResponseDto dtoResponse = await RegisterPassword();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.UserNotAuthenticated, dtoResponse.ResponseMessage);
    }
    #endregion

    #region Privates
    private async Task<BaseApiResponseDto> SendUserRegisterAuthCode()
    {
        UserRegisterDto dtoUserRegister = new()
                                          {
                                              UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                              UserName = AuthenticationTestConst.TEMP_NAME
                                          };

        BaseApiResponseDto dtoResponse = await facadeAuthentication.SendUserRegisterAuthCode(dtoUserRegister);

        return dtoResponse;
    }

    private async Task<BaseApiResponseDto> ValidateAuthCode()
    {
        UserAuthenticationDto? dtoUserAuthentication = entityUserAuthentication.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        ValidateAuthCodeDto dtoValidateAuthCode = new()
                                                  {
                                                      UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                                      AuthenticationCode = dtoUserAuthentication?.AuthCode ?? string.Empty
                                                  };

        BaseApiResponseDto dtoResponse = await facadeAuthentication.ValidateAuthCode(dtoValidateAuthCode);

        return dtoResponse;
    }

    private async Task<BaseApiResponseDto> RegisterPassword()
    {
        UserRegisterPasswordDto dtoRegisterPassword = new()
                                                      {
                                                          UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                                          Password = AuthenticationTestConst.TEMP_NAME + AuthenticationTestConst.TEMP_EMAIL
                                                      };

        BaseApiResponseDto dtoResponse = await facadeAuthentication.RegisterPassword(dtoRegisterPassword);

        return dtoResponse;
    }
    #endregion
}