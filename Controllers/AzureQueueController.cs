using Microsoft.AspNetCore.Mvc;
namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AzureQueueController : ControllerBase
{
    private readonly ILogger<AzureQueueController> _logger;
    private readonly IAzureQueue _azuerQueue;
    public AzureQueueController(IAzureQueue azureQueue, ILogger<AzureQueueController> logger)
    {
        _logger = logger;
        _azuerQueue = azureQueue;
    }
    [HttpPost("CreateQueueAsync")]
    public async Task<IActionResult> CreateQueueAsync(string queueName)
    {
        await _azuerQueue.CreateQueueAsync(queueName);
        return Ok("Queue Created");
    }
    [HttpPost("SendMessageAsync")]
    public async Task<IActionResult> SendMessageAsync(string queueName,string message)
    {
        await _azuerQueue.SendMessageAsync(queueName, message);
        return Ok("Message sent");
    }
    [HttpGet]
    public async Task<IActionResult> ReadQueueAsync(string queueName)
    {
        return Ok (await _azuerQueue.ReadQueueAsync(queueName));
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteQueueAsync(string queueName)
    {
        return Ok (await _azuerQueue.DeleteQueueAsync(queueName));
    }
    // [HttpPut]
    // public async Task<IActionResult> UpdateMessage(string queueName, string message)
    // {
    //     return Ok (await _azuerQueue.UpdateMessage(queueName,message));
    // }
}