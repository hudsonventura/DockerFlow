using System;
using Microsoft.OpenApi.Models;
using Shared.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API do Docker-LogFlow", Version = "1.0" });

    // Inclua o caminho para o arquivo XML gerado
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddCors();


var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
IConfiguration appsettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();
dependencies_injection(appsettings);


create_DataBase(appsettings);








var app = builder.Build();

app.UseCors(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void dependencies_injection(IConfiguration appsettings){
    builder.Services.AddSingleton<IConfiguration>(appsettings);

    builder.Services.AddDbContext<DataBaseContext>(options =>
    {
        options.UseNpgsql(appsettings.GetConnectionString("DefaultConnection"));
    });
}

void create_DataBase(IConfiguration appsettings){
    using (var dbContext = new DataBaseContext(appsettings))
    {
        try
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.Migrate();
            Console.WriteLine("Conexão bem-sucedida. O banco de dados existe e é válido.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao conectar ao banco de dados.Banco de dads está online? String de conexão está correta? Erro: {ex.Message}");
        }
    }
}