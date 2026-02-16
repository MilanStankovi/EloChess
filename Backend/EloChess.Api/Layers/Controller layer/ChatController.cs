using EloChess.Api.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace EloChess.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ChatController> _logger;

    public ChatController(KafkaProducer kafkaProducer, ILogger<ChatController> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequest request)
    {
        if (string.IsNullOrEmpty(request.Message))
        {
            return BadRequest("Poruka ne može biti prazna.");
        }

        try
        {
            // Šaljemo poruku na Aiven Kafku koristeći tvoj Producer
            // Koristimo Username kao ključ (Key) da bi poruke istog korisnika išle na istu particiju
            await _kafkaProducer.ProduceAsync(request.Username, request.Message);

            _logger.LogInformation("Poruka od {User} uspešno poslata na Kafku.", request.Username);
            
            return Ok(new { 
                Status = "Poslato", 
                Timestamp = DateTime.UtcNow,
                User = request.Username 
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška prilikom slanja poruke na Kafku.");
            return StatusCode(500, "Interna greška servera pri komunikaciji sa Kafkom.");
        }
    }
}

// Model za request
public record ChatMessageRequest(string Username, string Message);