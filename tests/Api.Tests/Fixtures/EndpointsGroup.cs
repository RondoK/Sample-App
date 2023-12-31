using System.Net.Http.Json;
using App.Data.Models;
using FastApi.Endpoints;
using SystemTextJsonPatch;

namespace Api.Tests.Fixtures;

public class EndpointsGroup<T> where T : class
{
    private readonly string _baseUri;
    private readonly EndpointsRequestUri _endpointsUri;
    private readonly HttpClient _api;

    public EndpointsGroup(string baseUri, HttpClient api)
    {
        _baseUri = baseUri;
        _api = api;
        _endpointsUri = new EndpointsRequestUri(_baseUri);
    }

    public async Task<T> Create(T request)
    {
        var response = await _api.PostAsJsonAsync(_baseUri, request);
        return await GetBodyOrException(response);
    }

    public async Task<T?> GetById<TId>(TId id)
    {
        return await _api.GetFromJsonAsync<T>(_endpointsUri.IdBased(id));
    }

    public async Task<Agg[]?> GetAll()
    {
        return await _api.GetFromJsonAsync<Agg[]>(_endpointsUri.GetAll());
    }

    public async Task<Agg[]?> GetPaged(int page, int pageSize)
    {
        return await _api.GetFromJsonAsync<Agg[]>(_endpointsUri.GetPaged(page, pageSize));
    }

    public async Task<T> Update(T updated)
    {
        var response = await _api.PutAsJsonAsync(_baseUri, updated);
        return await GetBodyOrException(response);
    }

    public async Task<T> Patch<TId>(TId id, JsonPatchDocument<T> patch)
    {
        return await GetBodyOrException( await OnlyPatch(id, patch));
    }
    
    public async Task<HttpResponseMessage> OnlyPatch<TId>(TId id, JsonPatchDocument<T> patch)
    {
        return await _api.PatchAsJsonAsync(_endpointsUri.IdBased(id), patch);
    }

    private async Task<T> GetBodyOrException(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            throw new Exception(
                $"Failed to create an item, response has status code {response.StatusCode}.\nAnd body: " +
                await response.Content.ReadAsStringAsync());
        var result = await response.Content.ReadFromJsonAsync<T>();
        if (result == null)
            throw new Exception("Failed to create an item, response can't be casted");
        return result;
    }
}

public static class EndpointsGroupExtension
{
    public static EndpointsGroup<T> GetDefaultEndpoints<T>(this ClientFixture fixture, string baseUri)
        where T : class
    {
        return new EndpointsGroup<T>(baseUri, fixture.Api);
    }
}