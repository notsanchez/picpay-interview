using Microsoft.OpenApi.Models;
using Npgsql;
using PicPaySimplificado.Infra.Repositories;
using PicPaySimplificado.Application.Services;
using PicPaySimplificado.Application.Interfaces;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
builder.Services.AddControllers();

var connectionString = configuration.GetConnectionString("PostgresConnection");

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionString));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PicPaySimplificado API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicPaySimplificado API V1");
    c.RoutePrefix = string.Empty;
});

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
