using CombinationGeneratorAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CombinationGeneratorAPI.UnitTests.Integration
{
    public class IntegrationTestFixture : IDisposable
    {
        public HttpClient Client { get; }
        public AppDbContext DbContext { get; }

        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTestFixture()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Replace real DbContext with in-memory DB
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                        if (descriptor != null) services.Remove(descriptor);

                        var dbName = Guid.NewGuid().ToString();
                        services.AddDbContext<AppDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(dbName)
                                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
                        });
                    });
                });

            Client = _factory.CreateClient();
            var scope = _factory.Services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            Client.Dispose();
            _factory.Dispose();
        }
    }
}