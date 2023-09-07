using System;
using System.Collections.Generic;

namespace MoonIntelligentAssistant.Data.Entities;

public partial class AuthUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime RegisterDate { get; set; }
}
