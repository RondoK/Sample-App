namespace App.Data.Postgres;

/// <summary>
/// Allows run ef tools, "ef migrations" and "ef database".
/// Accepts "-c" or "--connectionString" parameter.
/// Examples :
/// - dotnet ef migrations add New --project App.Data.Postgres
/// - dotnet ef database update --project App.Data.Postgres
/// </summary>
public class DesignTimeFactory : DefaultDesignTimeFactory
{
    private const string DefaultConnectionString =
        @"Server=127.0.0.1;Port=5432;Database=the_app;User Id=postgres;Password=mysecretpassword;";

    public DesignTimeFactory() : base(DefaultConnectionString, new PostgresContextParamsFactory(DefaultConnectionString))
    {
    }
}