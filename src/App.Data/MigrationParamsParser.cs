using System.CommandLine;

namespace App.Data;

public static class MigrationParamsParser
{
    public static string GetConnectionStringOrDefault(string[] args, string defaultConnectionString)
    {
        var connectionString  = "";
        var connStringOption = new Option<string>(new[] { "--connectionString", "-c" },
            () => defaultConnectionString,
            "Overrides connection string");
        var rootCommand = new RootCommand("Accept optional params");
        rootCommand.AddOption(connStringOption);
        rootCommand.SetHandler((connString) =>
            {
                connectionString = connString;
            },
            connStringOption);
        rootCommand.Invoke(args);
        return connectionString;
    }
}