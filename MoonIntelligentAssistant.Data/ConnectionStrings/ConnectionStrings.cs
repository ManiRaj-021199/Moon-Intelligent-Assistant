namespace MoonIntelligentAssistant.Data;

public static class ConnectionStrings
{
    #region Constants
    public static readonly string DbConnectionString = $"Server={EntityConstants.Server};Database={EntityConstants.InitialCatelog};Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;";
    #endregion

    // Run the following in the Package Manager Console for Migrate the Db
    // Scaffold-DbContext "Server=<Server Name>;Database=Moon-IA;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -ContextDir DBContext -force
}

public static class EntityConstants
{
    #region Constants
    public static readonly string Server = "MS-NB0101";
    public static readonly string InitialCatelog = "Moon-IA";
    #endregion
}