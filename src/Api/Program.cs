using Api;
using Api.Endpoints;
using App.Data;
using App.Data.Sqlite;
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
        AddSqlLiteContext(builder.Services, builder.Configuration.GetConnectionString("Sqlite") ?? throw new Exception("No connection string"));
        builder.Services.AddScoped<DbContext, Context>();
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin));

        builder.Services.AddSingleton<IConfigureModelCreating>(new ConfigureSqlite());
        
        return builder;
    }

    public static IServiceCollection AddSqlLiteContext(IServiceCollection service, string connectionString)
    {
        return service.AddDbContext<Context>(o =>
            o.UseSqlite(connectionString,
                c => c.MigrationsAssembly("App.Data.Sqlite"))
        );
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