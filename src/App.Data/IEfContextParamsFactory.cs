using Microsoft.EntityFrameworkCore;

namespace App.Data;

public interface IEfContextParamsFactory
{
    public void SetConnectionString(string connectionString);
    public Action<DbContextOptionsBuilder> BuildOptionsDelegate();
    public IConfigureModelCreating CreateModelCreatingOptions();
}