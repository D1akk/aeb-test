using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using client.Models;

namespace client.Controllers
{
    public class MessageClientController : Controller
    {
        private readonly HttpClient _httpClient;

        public MessageClientController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

    
        [HttpGet("/client1")]
        public IActionResult Client1()
        {
            return View();
        }

        [HttpPost("/client1")]
        public async Task<IActionResult> SendMessage(Message message)
        {
            var jsonContent = JsonSerializer.Serialize(message);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://api:8080/api/message/send-message", content);
            if (response.IsSuccessStatusCode)
            {
                ViewBag.MessageStatus = "Сообщение успешно отправлено!";
            }
            else
            {
                ViewBag.MessageStatus = "Ошибка.";
            }

            return View("Client1");
        }

        [HttpGet("/client2")]
        public IActionResult Client2()
        {
            return View();
        }

        [HttpGet("/client3")]
        public IActionResult Client3()
        {
            return View();
        }

        [HttpPost("/client3")]
        public async Task<IActionResult> GetMessagesHistory()
        {
          
            var response = await _httpClient.GetAsync("http://api:8080/api/message/messages");
            if (response.IsSuccessStatusCode)
            {
                var messages = await response.Content.ReadAsStringAsync();
                ViewBag.MessageHistory = JsonSerializer.Deserialize<List<Message>>(messages);
            }
            else
            {
                ViewBag.MessageHistory = "Failed to retrieve message history.";
            }

            return View("Client3");
        }
    }
}
