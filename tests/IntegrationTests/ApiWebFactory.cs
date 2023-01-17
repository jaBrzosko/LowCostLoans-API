using Api;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using IntegrationTests.MockedServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Services.Configurations;
using Services.Data;
using Services.Services.BlobStorages;
using Xunit;

namespace IntegrationTests;

public class ApiWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly TestcontainerDatabase database = new TestcontainersBuilder<PostgreSqlTestcontainer>()
        .WithDatabase(new PostgreSqlTestcontainerConfiguration
        {
            Database = "testDb",
            Username = "testUser",
            Password = "doesnt_matter",
        }).Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());

        builder.ConfigureAppConfiguration(cfg =>
        {
            cfg.Properties.Add("FrontendPrefix", "http://localhost:3000");
            cfg.Properties.Add("JWTSigningKey", "JWTSigningKeyJWTSigningKeyJWTSigningKeyJWTSigningKeyJWTSigningKeyJWTSigningKey");
        });

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<CoreDbContext>));
            services.Remove(descriptor);

            services.RemoveAll<CoreDbContext>();
            services.AddDbContext<CoreDbContext>(
                opts => opts.UseNpgsql(database.ConnectionString)
            );

            services.RemoveAll<BlobStorageConfiguration>();
            services.RemoveAll<ApiKeyConfiguration>();
            services.RemoveAll<BlobStorage>();
            
            services.AddSingleton(new BlobStorageConfiguration("blob"));
            services.AddSingleton(new ApiKeyConfiguration("api-key"));
            services.AddTransient<IBlobStorage, MockedBlobStorage>();
        });
    }
    
    public async Task InitializeAsync()
    {
        await database.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await database.DisposeAsync();
    }
}
