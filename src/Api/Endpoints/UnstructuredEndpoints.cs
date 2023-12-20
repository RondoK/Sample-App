using System.Security.Claims;
using App.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using FastApi.Endpoints;

namespace Api.Endpoints;

public static class UnstructuredEndpoints
{
    public static void ConfigureMinimalEndpoints(WebApplication app)
    {
        app.MapGet("/user", (HttpContext ctx) => ctx.User.FindFirst("usr")?.Value ?? "empty");
        app.MapGet(Paths.Login, async (HttpContext ctx) =>
        {
            var claims = new List<Claim>() { new Claim("usr", "Dan"), new Claim("pass", "eu") };
            var identity = new ClaimsIdentity(claims, ApiBuilder.CookieScheme);
            var user = new ClaimsPrincipal(identity);

            await ctx.SignInAsync(ApiBuilder.CookieScheme, user);
            return "ok";
        });
        app.Map(Paths.Admin.AttributeProtected, [Authorize(Roles = Roles.Admin)](HttpContext ctx) => "ok");
        app.Map(Paths.Admin.FluentApiProtected, (HttpContext ctx) => "ok").RequireAuthorization(Roles.Admin);
        app.MapGet(Paths.Resource, () => "ok").RequireAuthorization();


        
        // app endpoints can be wrapped to add a url part before already defined url 
        // use case: Add version to an api 
        //var routes = app.MapGroup("v1");
        var routes = app;
        routes.MapGroupAndSingleActions<Agg, int>(Paths.Aggs); //.RequireAuthorization();

    }
}