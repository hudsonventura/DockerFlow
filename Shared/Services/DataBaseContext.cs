using System.Collections.Generic;
using DockerFlow.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shared.Services;

public class DataBaseContext : DbContext
{
    public DbSet<CommonLog> logs { get; set; }

    public DbSet<SystemLog> system_logs { get; set; }


    private readonly IConfiguration _configuration;

    public DataBaseContext()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
            .Build();
        Console.WriteLine(Directory.GetCurrentDirectory());
    }

    public DataBaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
            //Console.WriteLine($"Tentando conectar ao banco {connectionString} ... Sucesso!");
        }
    }


    
}
