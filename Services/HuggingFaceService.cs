using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EcosferaDigitalAPI.Services
{
    public class HuggingFaceService
    {
        private readonly string _apiUrl = "https://api-inference.huggingface.co/models/gpt-2";
        private readonly HttpClient _httpClient;

        // Construtor que exige a chave da API
        public HuggingFaceService(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("A chave da API é obrigatória.", nameof(apiKey));
            }

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
        }

        // Método para gerar texto com o modelo HuggingFace
        public async Task<string> GenerateTextAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentException("O prompt não pode ser vazio.", nameof(prompt));
            }

            var requestData = new { inputs = prompt };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            // Tentativa de enviar a requisição
            var response = await _httpClient.PostAsync(_apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro na solicitação: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApiResponse>(responseString);

            if (result == null || string.IsNullOrWhiteSpace(result.GeneratedText))
            {
                throw new Exception("Resposta inesperada ou sem texto gerado da API.");
            }

            return result.GeneratedText;
        }
    }

    // Classe para mapear a resposta da API
    public class ApiResponse
    {
        [JsonProperty("generated_text")]
        public string GeneratedText { get; set; } = string.Empty;
    }
}
