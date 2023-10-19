using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests;

public class TestProgram
{
    public static Task Main(string[] args)
    {
        CreateApp(args).Run();
        return Task.CompletedTask;
    }

    private static WebApplication CreateApp(params string[] args)
    {
        // For some reasons Rider passes old path for the project
        var envParams = args.Where(a => !a.Contains("--contentRoot")).ToArray();
        var app = ApiBuilder.AddServices(envParams).Build();
        ApiBuilder.ConfigureMiddleware(app);
        app.MapGet(TestPaths.TestLogin, async ([FromQuery] string[] roles, HttpContext ctx) =>
        {
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                    roles.Select(r => new Claim(ClaimTypes.Role, r)),
                    ApiBuilder.CookieScheme));
            await ctx.SignInAsync(ApiBuilder.CookieScheme, user);
            return "ok";
        });
        return app;
    }
}

public static class Extensions
{
    public static async Task<HttpResponseMessage> MockedLogin(this HttpClient client, params string[] roles)
    {
        //TODO : find prettier way pass query params 
        var path = TestPaths.TestLogin +
                   string.Concat(roles.Skip(1).Select(r => "&" + r).Prepend(roles.First()).Prepend("?roles="));
        return await client.GetAsync(path);
    }
}