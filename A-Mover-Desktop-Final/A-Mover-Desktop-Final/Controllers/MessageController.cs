using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace A_Mover_Desktop_Final.Controllers
{
    public class MessageController : Controller
    {
        private readonly IConfiguration _configuration;

        public MessageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult SendMessage()
        {
            // Passar a API Key para a view
            ViewBag.ApiKey = _configuration["MessageAPI:ApiKey"];
            ViewBag.BaseUrl = _configuration["MessageAPI:BaseUrl"];
            return View();
        }
    }
}