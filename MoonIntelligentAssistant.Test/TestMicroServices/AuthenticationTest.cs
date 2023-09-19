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
        facadeAuthentication = new AuthenticationFacade(entityLog, entityAuthUsers, entityNonAuthUsers, activityDbContext);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [User].AuthUsers");
        dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE [User].NonAuthUsers");

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

        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreEqual(AuthenticationTestConst.TEMP_NAME, dtoNonAuthUser.UserName);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoNonAuthUser.UserEmail);
        Assert.IsFalse(dtoNonAuthUser.IsAuthenticated);
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

        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoNonAuthUser.UserEmail);

        DateTime dtFirstUpdate = dtoNonAuthUser.RegisterDate;

        // Act
        await SendUserRegisterAuthCode();

        // Assert
        dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoNonAuthUser.UserEmail);
        Assert.AreNotEqual(dtFirstUpdate, dtoNonAuthUser.RegisterDate);
    }

    [TestMethod]
    public async Task Test_SendUserRegisterAuthCode_AuthCodeExpired()
    {
        // Arrange
        await SendUserRegisterAuthCode();

        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        Assert.IsNotNull(dtoNonAuthUser);

        string strExpiredAuthCode = dtoNonAuthUser.AuthCode;

        dtoNonAuthUser.RegisterDate -= new TimeSpan(0, 5, 1);

        entityNonAuthUsers.NonAuthUsersWriter.Update(dtoNonAuthUser);
        await activityDbContext.PersistAsync();

        // Act
        BaseApiResponseDto dtoResponse = await SendUserRegisterAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationSuccessMessages.SendAuthenticationCode, dtoResponse.ResponseMessage);

        dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreNotEqual(strExpiredAuthCode, dtoNonAuthUser.AuthCode);
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

        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoNonAuthUser.UserEmail);
        Assert.IsTrue(dtoNonAuthUser.IsAuthenticated);
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

        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        Assert.IsNotNull(dtoNonAuthUser);

        string strExpiredAuthCode = dtoNonAuthUser.AuthCode;

        dtoNonAuthUser.RegisterDate -= new TimeSpan(0, 5, 1);

        entityNonAuthUsers.NonAuthUsersWriter.Update(dtoNonAuthUser);
        await activityDbContext.PersistAsync();

        // Act
        BaseApiResponseDto dtoResponse = await ValidateAuthCode();

        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, dtoResponse.ResponseCode);
        Assert.AreEqual(AuthenticationErrorMessages.ExpiredAuthCode, dtoResponse.ResponseMessage);

        dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoNonAuthUser);
        Assert.AreNotEqual(strExpiredAuthCode, dtoNonAuthUser.AuthCode);
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

        AuthUserDto? dtoAuthUser = entityAuthUsers.AuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);
        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        Assert.IsNotNull(dtoAuthUser);
        Assert.IsNull(dtoNonAuthUser);

        Assert.AreEqual(AuthenticationTestConst.TEMP_NAME, dtoAuthUser.UserName);
        Assert.AreEqual(AuthenticationTestConst.TEMP_EMAIL, dtoAuthUser.UserEmail);
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
        NonAuthUserDto? dtoNonAuthUser = entityNonAuthUsers.NonAuthUsersReader.GetByEmail(AuthenticationTestConst.TEMP_EMAIL);

        ValidateAuthCodeDto dtoValidateAuthCode = new()
                                                  {
                                                      UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                                      AuthenticationCode = dtoNonAuthUser?.AuthCode ?? string.Empty
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