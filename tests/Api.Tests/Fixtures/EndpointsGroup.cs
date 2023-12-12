using System.Net.Http.Json;
using App.Data.Models;
using FastApi.Endpoints;

namespace Api.Tests.Fixtures;

public class EndpointsGroup<T>
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
        var addResponse = await _api.PostAsJsonAsync(_baseUri, request);
        if (!addResponse.IsSuccessStatusCode)
            throw new Exception(
                $"Failed to create an item, response has status code {addResponse.StatusCode}.\nAnd body: " +
                await addResponse.Content.ReadAsStringAsync());
        var result = await addResponse.Content.ReadFromJsonAsync<T>();
        if (result == null)
            throw new Exception("Failed to create an item, response can't be casted");
        return result;
    }

    public async Task<T?> GetById<TId>(TId id)
    {
        return await _api.GetFromJsonAsync<T>(_endpointsUri.GetById(id));
    }

    public async Task<Agg[]?> GetAll()
    {
        return await _api.GetFromJsonAsync<Agg[]>(_endpointsUri.GetAll());
    }

    public async Task<Agg[]?> GetPaged(int page, int pageSize)
    {
        return await _api.GetFromJsonAsync<Agg[]>(_endpointsUri.GetPaged(page, pageSize));
    }
}

public static class EndpointsGroupExtension
{
    public static  EndpointsGroup<T> GetDefaultEndpoints<T>(this ClientFixture fixture, string baseUri)
    {
        return new EndpointsGroup<T>(baseUri, fixture.Api);
    }
}