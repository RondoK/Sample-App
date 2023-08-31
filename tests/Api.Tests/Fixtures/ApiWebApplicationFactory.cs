using App.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests.Fixtures;

//xUnit needs this class to be public
// ReSharper disable once ClassNeverInstantiated.Global
public class ApiWebApplicationFactory : WebApplicationFactory<TestProgram>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .Build();

            config.AddConfiguration(configuration);
        });
        
        // Register services 

        builder.ConfigureTestServices(serviceCollection => 
            ApiBuilder.AddSqlLiteContext(serviceCollection, "DataSource=:memory:"));

    }
    
    public IServiceScope CreateScope()
    {
        return Services.CreateScope();
    }

    public Context GetScopedContext()
    {
        return CreateScope().ServiceProvider.GetService<Context>()!;
    }
}
