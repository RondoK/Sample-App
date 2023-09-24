namespace App.Data.SqlServer;

/// <summary>
/// Allows run ef tools, "ef migrations" and "ef database".
/// Accepts "-c" or "--connectionString" parameter.
/// Examples :
/// - dotnet ef migrations add New --project App.Data.SqlServer
/// - dotnet ef database update --project App.Data.SqlServer
/// </summary>
public class DesignTimeFactor : DefaultDesignTimeFactory
{
    private const string DefaultConnectionString =
        @"Data Source=.; Database=TheApp; User Id=sa; Password=your(!)Password; Trust Server Certificate =Yes";

    public DesignTimeFactor() : base(DefaultConnectionString, new SqlServerContextParamsFactory(DefaultConnectionString))
    {
    }
}