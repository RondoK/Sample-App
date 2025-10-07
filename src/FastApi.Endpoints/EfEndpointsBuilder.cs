using System.ComponentModel.DataAnnotations;
using FastApi.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SystemTextJsonPatch;

namespace FastApi.Endpoints;

public static class EfEndpointsBuilder
{
    public static IEndpointConventionBuilder MapGroupAndSingleActions<T, TId>(this IEndpointRouteBuilder routeBuilder,
        string prefix)
        where T : class
    {
        var group = routeBuilder.MapGroup(prefix);
        group.MapSingleItemGroup<T, TId>();
        group.MapGroupActions<T>();
        return group;
    }

    /// <summary>
    /// Minimal Api endpoints with default(for this project) routes and FastApi Services.
    /// Includes only actions over collection of entities (Get all, get paged, add one new, update)
    /// </summary>
    public static RouteGroupBuilder MapGroupActions<T>(this RouteGroupBuilder group)
        where T : class
    {
        group.GetAll<T>("/all");
        group.GetPaged<T>();
        //Create
        group.AddOneNew<T>();
        group.Update<T>();
        return group;
    }

    /// <summary>
    /// Minimal Api endpoints with default(for this project) routes and FastApi Services.
    /// Includes ONLY actions over SINGLE entity (get single entity, put single entity, patch single entity)
    /// </summary>
    public static RouteGroupBuilder MapSingleItemGroup<T, TId>(this RouteGroupBuilder outer)
        where T : class
    {
        var endpoints = new SingleEntityRoutes<T, TId>(outer.MapGroup("/{id}"));
        endpoints.GetById();
        endpoints.Put();
        endpoints.Patch();
        return endpoints.GroupBuilder;
    }

    public static RouteHandlerBuilder GetAll<T>(this RouteGroupBuilder group, string routePattern = "/")
        where T : class =>
        group.MapGet(routePattern, (DbContext context) => context.GetAll<T>());

    public static RouteHandlerBuilder GetPaged<T>(this RouteGroupBuilder group, string routePattern = "/")
        where T : class =>
        /*
         TODO : explore easy way to validate query parameters.
         Looks like System.ComponentModel.DataAnnotations Range gives only hint in swagger,but no real validation
        */
        group.MapGet(routePattern,
            async Task<IResult>([FromServices]IProvidePaged<T> provider, int page, /*[Range(1, 5)]*/ int pageSize) =>
            {
                //TODO: add proper validation pattern
                //TODO: make configurable 
                if (pageSize < 1)
                    return TypedResults.BadRequest("Not valid page size. Page size expected to be > 1");

                //TODO: add proper validation pattern
                //TODO: make configurable 
                if (pageSize > 10)
                    return TypedResults.BadRequest("Not valid page size. Page size expected to be < 11");
                
                //TODO : Add ordering
                return TypedResults.Ok(await provider.GetPageAsync(page, pageSize));
            });

    public static RouteHandlerBuilder AddOneNew<T>(this RouteGroupBuilder group, string routePattern = "/")
        where T : class =>
        group.MapPost(routePattern, (DbContext context, [FromBody] T entity) => context.SaveNewAsync(entity));

    public static RouteHandlerBuilder Update<T>(this RouteGroupBuilder group, string routePattern = "/")
        where T : class =>
        group.MapPut(routePattern, (DbContext context, [FromBody] T entity) => context.SaveUpdateAsync(entity));
    
    
    public class SingleEntityRoutes<T, TId> where T : class
    {
        public RouteGroupBuilder GroupBuilder { get; }

        public SingleEntityRoutes(RouteGroupBuilder groupBuilder)
        {
            GroupBuilder = groupBuilder;
        }

        public RouteHandlerBuilder GetById(string routePattern = "") =>
            GroupBuilder.MapGet(routePattern,
                ([FromServices] DbContext context, TId id) => context.FindNoTrackingAsync<T>(id!));

        public RouteHandlerBuilder Put(string routePattern = "") =>
            GroupBuilder.MapPut("",
                ([FromServices] DbContext context,
                    //TODO: check id mismatch between url id and body id
                    TId id,
                    [FromBody] T entity) => context.SaveUpdateAsync(entity));

        public RouteHandlerBuilder Patch(string routePattern = "") =>
            GroupBuilder.MapPatch("",
                async ([FromServices] DbContext context, [FromServices] NonEditableProperties patchValidation, TId id,
                    [FromBody] JsonPatchDocument<T> patch) =>
                {
                    var stored = await context.FindTracked<T>(id!);
                    if (!patchValidation.IsValidPatch(patch))
                        return Results.BadRequest();
                    patch.ApplyTo(stored!);
                    await context.SaveChangesAsync();
                    return Results.Ok(stored);
                });
    }
}
