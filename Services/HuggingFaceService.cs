using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EcosferaDigitalAPI.Services
{
    public class HuggingFaceService
    {
        private readonly string _apiUrl = "https://api-inference.huggingface.co/models/gpt-2"; // Exemplo de modelo GPT-2
        private readonly string _apiKey; // Chave da API, não pode ser nula

        private readonly HttpClient _httpClient;

        // Construtor que exige a chave da API
        public HuggingFaceService(string apiKey)
        {
            // Verificando se a chave da API não é fornecida
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("A chave da API é obrigatória.", nameof(apiKey));
            }

            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}"); // Definindo o cabeçalho da API
        }

        // Método para gerar texto com o modelo HuggingFace
        public async Task<string> GenerateTextAsync(string prompt)
        {
            // Verificando se o prompt está vazio ou nulo
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentException("O prompt não pode ser vazio.", nameof(prompt));
            }

            var requestData = new
            {
                inputs = prompt
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Fazendo a solicitação para a API HuggingFace
            var response = await _httpClient.PostAsync(_apiUrl, content);

            // Se a resposta for bem-sucedida, processa os dados
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ApiResponse[]>(responseString); // Deserializando para um array de ApiResponse

                // Verifica se a resposta contém o texto gerado
                string? generatedText = result?.FirstOrDefault()?.GeneratedText;

                // Se o texto gerado for nulo ou vazio, lança uma exceção
                if (string.IsNullOrWhiteSpace(generatedText))
                {
                    throw new Exception("Resposta inesperada ou sem texto gerado da API.");
                }

                return generatedText ?? string.Empty; // Retorna o texto gerado ou uma string vazia se nulo
            }
            else
            {
                throw new Exception($"Erro na solicitação: {response.StatusCode}");
            }
        }
    }

    // Classe para mapear a resposta da API
    public class ApiResponse
    {
        [JsonProperty("generated_text")]
        public string GeneratedText { get; set; } = string.Empty;
    }
}
