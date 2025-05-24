namespace FastApi.EF;

public interface IProvidePaged<T>
{
    Task<List<T>> GetPageAsync(int page, int pageSize);
}