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
    public async Task Test_RegisterPassword_Success()
    {
        // Arrange
        await SendUserRegisterAuthCode();
        await ValidateAuthCode();

        UserRegisterPasswordDto dtoRegisterPassword = new()
                                                      {
                                                          UserEmail = AuthenticationTestConst.TEMP_EMAIL,
                                                          Password = AuthenticationTestConst.TEMP_NAME + AuthenticationTestConst.TEMP_EMAIL
                                                      };

        // Act
        BaseApiResponseDto dtoResponse = await facadeAuthentication.RegisterPassword(dtoRegisterPassword);

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
        Assert.IsNotNull(dtoNonAuthUser);

        ValidateAuthCodeDto dtoValidateAuthCode = new()
                                                  {
                                                      UserEmail = dtoNonAuthUser.UserEmail,
                                                      AuthenticationCode = dtoNonAuthUser.AuthCode
                                                  };

        BaseApiResponseDto dtoResponse = await facadeAuthentication.ValidateAuthCode(dtoValidateAuthCode);

        return dtoResponse;
    }
    #endregion
}