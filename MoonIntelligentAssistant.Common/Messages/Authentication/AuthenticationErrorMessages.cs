﻿namespace MoonIntelligentAssistant.Common;

public static class AuthenticationErrorMessages
{
    #region Properties
    public static string InvalidEmail => "Invalid Email Address. Please check your email address...";
    public static string UserNotRegistered => "User not registered with us. Please register to continue...";
    public static string UserAlreadyRegistered => "User already registered with us. Please login to continue...";
    public static string UserNotAuthenticated => "User not authenticated. Please register to continue...";
    public static string WrongAuthCode => "Wrong authentication code. Please enter the correct authentication code...";
    public static string ExpiredAuthCode => "Authentication code was expired. Please check your mail for new auth code...";
    #endregion
}