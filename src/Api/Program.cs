using Api;
using Api.Endpoints;
using App.Data;
using App.Data.Postgres;
using App.Data.Sqlite;
using App.Data.SqlServer;
using Microsoft.EntityFrameworkCore;

ApiBuilder.CreateApp(args).Run();

public static class ApiBuilder
{
    public const string CookieScheme = "cookie";

    public static WebApplication CreateApp(params string[] args)
    {
        var app = AddServices(args).Build();
        ConfigureMiddleware(app);
        return app;
    }

    public static WebApplicationBuilder AddServices(params string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAuthentication(o => o.DefaultScheme = CookieScheme)
            .AddCookie(CookieScheme);
        builder.AddEfContext();
        builder.Services.AddScoped<DbContext, Context>();
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));

        return builder;
    }

    private static void AddEfContext(this WebApplicationBuilder builder)
    {
        var factory = GetParamsFactory(builder.Configuration);
        builder.Services.AddSingleton<IConfigureModelCreating>(factory.CreateModelCreatingOptions());
        builder.Services.AddDbContext<DbContext,Context>(factory.BuildOptionsDelegate());
    }

    private static IEfContextParamsFactory GetParamsFactory(IConfiguration configuration)
    {
        var provider = configuration.GetValue("DbProvider", "Sqlite");
        if (provider == null)
            throw new Exception("Missing config parameter DbProvider");
        var connectionString = configuration.GetConnectionString(provider);
        if (connectionString == null)
            throw new Exception("Missing connection string named " + provider);
        
        return provider switch
        {
            "Sqlite" => new SqliteContextParamsFactory(connectionString),
            "SqlServer" => new SqlServerContextParamsFactory(connectionString),
            "Postgres" => new PostgresContextParamsFactory(connectionString),
            _ => throw new Exception($"Unsupported provider: {provider}")
        };
    }

    public static WebApplication ConfigureMiddleware(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // TODO: Test Cors 
        app.UseCors();
        /*
        app.UseHttpsRedirection();
        app.UseHsts()
        
        app.UseAuthentication();
        app.UseAuthorization();

        */

        UnstructuredEndpoints.ConfigureMinimalEndpoints(app);

        return app;
    }
}