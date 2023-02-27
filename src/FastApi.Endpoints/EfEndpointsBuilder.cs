using System.ComponentModel.DataAnnotations;
using FastApi.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace FastApi.Endpoints;

public static class EfEndpointsBuilder
{
    public static IEndpointConventionBuilder MapGroupAndSingleActions<T>(this IEndpointRouteBuilder routeBuilder, string prefix)
        where T : class
    {
        var group = routeBuilder.MapGroup(prefix);
        group.MapSingleItemGroup<T>();
        group.MapGroupActions<T>();
        return group;
    }

    public static RouteGroupBuilder MapGroupActions<T>(this RouteGroupBuilder group)
        where T : class
    {
        group.MapGet("/all", (DbContext context) => context.GetAll<T>());
        /*TODO : explore easy way to validate query parameters. Looks like System.ComponentModel.DataAnnotations Range gives only hint in swagger,but no real validation
        */
        group.MapGet("/",
            (DbContext context, int page, [Range(1, 5)] int pageSize) => context.GetPageAsync<T>(page, pageSize));
        //Create
        group.MapPost("/", (DbContext context, [FromBody] T entity) => context.SaveNewAsync(entity));
        group.MapPut("/", (DbContext context, [FromBody] T entity) => context.SaveUpdateAsync(entity));
        return group;
    }

    public static RouteGroupBuilder MapSingleItemGroup<T>(this RouteGroupBuilder outer)
        where T : class
    {
        var inner = outer.MapGroup("/{id}");
        inner.MapGet("", ([FromServices] DbContext context, int id) => context.FindNoTrackingAsync<T>(id));
        inner.MapPut("",
            ([FromServices] DbContext context, int id, [FromBody] T entity) => context.SaveUpdateAsync(entity));
        return inner;
    }
}