using System;
using System.Collections.Generic;

namespace MoonIntelligentAssistant.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public byte FailedLoginCount { get; set; }

    public DateTime RegisterDate { get; set; }
}
