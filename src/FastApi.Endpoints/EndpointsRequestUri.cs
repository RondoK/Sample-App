namespace FastApi.Endpoints;

public class EndpointsRequestUri
{
    private readonly string _uriBase;

    public EndpointsRequestUri(string uriBase)
    {
        _uriBase = uriBase;
    }

    public string GetById<TId>(TId id) => $"{_uriBase}/{id}";
    public string GetAll() => _uriBase + "/all";
    public string GetPaged(int neededPage, int pageSize) => $"{_uriBase}?page={neededPage}&pageSize={pageSize}";
}