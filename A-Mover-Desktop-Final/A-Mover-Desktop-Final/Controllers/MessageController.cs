using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace A_Mover_Desktop_Final.Controllers
{
    public class MessageController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public MessageController(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        public IActionResult SendMessage()
        {
            // Passar a API Key para a view
            ViewBag.ApiKey = _configuration["MessageAPI:ApiKey"];
            ViewBag.BaseUrl = _configuration["MessageAPI:BaseUrl"];
            return View();
        }
        public IActionResult TestarConexao()
        {
            // Passar a API Key para a view
            ViewBag.ApiKey = _configuration["MessageAPI:ApiKey"];
            ViewBag.BaseUrl = _configuration["MessageAPI:BaseUrl"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> TestConnection()
        {
            try
            {
                // Obter configurações da API
                string baseUrl = _configuration["MessageAPI:BaseUrl"];
                string apiKey = _configuration["MessageAPI:ApiKey"];

                if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
                {
                    return Json(new { success = false, message = "Configuração da API incompleta. Verifique o appsettings.json." });
                }

                // Criar cliente HTTP
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("x-api-key", apiKey);

                // Definir um ID de teste e uma mensagem de teste
                string testRecipientId = "TEST123456789";
                var payload = new
                {
                    message = "Testando conexão com MessageAPI",
                    sender = "TestApp"
                };

                // Converter para JSON
                var content = new StringContent(
                    JsonSerializer.Serialize(payload),
                    Encoding.UTF8,
                    "application/json");

                // Enviar requisição de teste
                var response = await client.PostAsync(
                    $"{baseUrl}/api/message/{testRecipientId}",
                    content);

                // Verificar resposta
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Conexão com a API bem-sucedida!" });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = $"Erro na conexão: {response.StatusCode} - {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Exceção ao conectar com a API: {ex.Message}" });
            }
        }
    }
}