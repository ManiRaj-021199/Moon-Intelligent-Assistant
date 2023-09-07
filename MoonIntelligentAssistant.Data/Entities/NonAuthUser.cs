using System;
using System.Collections.Generic;

namespace MoonIntelligentAssistant.Data.Entities;

public partial class NonAuthUser
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string AuthCode { get; set; } = null!;

    public DateTime RegisterDate { get; set; }
}
