using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EcosferaDigitalAPI.Services; // Adicione o namespace correto para HuggingFaceService

var builder = WebApplication.CreateBuilder(args);

// Adicionando a configuração de conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("OracleDbConnection");

// Registrando o serviço HuggingFace
builder.Services.AddSingleton<HuggingFaceService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Process.Start(new ProcessStartInfo("cmd", $"/c start http://localhost:5105/swagger/index.html") { CreateNoWindow = true });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
