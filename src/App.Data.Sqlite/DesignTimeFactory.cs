namespace App.Data.Sqlite;

/// <summary>
/// Allows run ef tools, "ef migrations" and "ef database".
/// Accepts "-c" or "--connectionString" parameter.
/// Examples :
/// - dotnet ef migrations add New --project App.Data.Sqlite -- -c "DataSource=../Api/app.db"
/// - dotnet ef database update --project App.Data.Sqlite -- -c "DataSource=../Api/app.db"
/// </summary>
public class DesignTimeFactory : DefaultDesignTimeFactory
{
    private const string DefaultConnectionString = "DataSource=../Api/app.db";

    public DesignTimeFactory() : base(DefaultConnectionString, new SqliteContextParamsFactory(DefaultConnectionString))
    {
    }
}