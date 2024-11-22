using System.Diagnostics;
using EcosferaDigitalAPI.Services; // Namespace do HuggingFaceService
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Adicionando a configuração de conexão com o banco de dados
var connectionString = builder.Configuration.GetConnectionString("OracleDbConnection");

// Adicionando o serviço HttpClient
builder.Services.AddHttpClient<HuggingFaceService>(client =>
{
    // Obtém a chave da API do appsettings.json
    var apiKey = builder.Configuration["HuggingFace:ApiKey"];
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new InvalidOperationException("A chave da API do HuggingFace não foi configurada.");
    }

    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
});

// Configurando os serviços da aplicação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Configuração para ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Process.Start(new ProcessStartInfo("cmd", $"/c start http://localhost:5105/swagger/index.html") { CreateNoWindow = true });
}

// Configuração do pipeline de requisições
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
