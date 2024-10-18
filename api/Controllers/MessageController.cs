using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    using api.Models;
    using api.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromBody] Message message)
        {
            await _messageRepository.AddMessageAsync(message);
            return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
        }

        [HttpGet("messages")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            var messages = await _messageRepository.GetMessagesAsync();
            return Ok(messages);
        }
    }


}
