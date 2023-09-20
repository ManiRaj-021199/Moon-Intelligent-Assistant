using System;
using System.Collections.Generic;

namespace MoonIntelligentAssistant.Data.Entities;

public partial class Error
{
    public int LogId { get; set; }

    public string ApiName { get; set; } = null!;

    public string ApiRequest { get; set; } = null!;

    public string Exception { get; set; } = null!;

    public DateTime LogDate { get; set; }
}
