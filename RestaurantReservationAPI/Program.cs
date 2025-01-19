using Microsoft.EntityFrameworkCore;
using RestaurantReservationAPI.Models;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis de ambiente do arquivo .env
DotNetEnv.Env.Load();

// Obter a string de conexão do ambiente ou de um fallback
var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' was not found in environment variables.");
}

// Adiciona serviços ao container
builder.Services.AddControllers();

// Configuração do DbContext com SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuração para Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuração do pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();