using System.Threading.Channels;
using Testcontainers.PostgreSql;

namespace Api.Tests;

public static class ChannelHolder
{
    public static Channel<PostgreSqlContainer> DbChannel { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    static ChannelHolder()
    {
        var env = Environment.GetEnvironmentVariable("DB_COUNT");
        Console.WriteLine("DB_COUNT PARAM " + env);
        if (!int.TryParse(env, out var count))
            count = 2;
        if (count < 1)
            throw new Exception($"DB_COUNT can't be < 1, provided value: \"{env}\"");

        InitChannel(count);
    }

    private static void InitChannel(int dbCount)
    {
        DbChannel = Channel.CreateBounded<PostgreSqlContainer>(dbCount);
        var containers = Enumerable.Range(1, dbCount)
            .Select(_ => new PostgreSqlBuilder()
                .WithUsername("postgres")
                .WithPassword("mysecretpassword")
                .WithDatabase("the_app")
                .Build()).ToArray();

        Task.WaitAll(containers.Select(c => c.StartAsync()).ToArray());
        foreach (var container in containers)
            DbChannel.Writer.TryWrite(container);
    }
}