using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configurations;

public sealed class QueryContextPostgres(
    IConfiguration configuration
) : QueryContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(BuildConnectionString());
    }
    
    private string BuildConnectionString()
    {
        string connectionString = configuration.GetValue<string>("Database:ConnectionString")!;
        string username = configuration.GetValue<string>("Database:Username")!;
        string password = configuration.GetValue<string>("Database:Password")!;

        return connectionString.Replace("devuser", username).Replace("devpassword", password);
    }
}