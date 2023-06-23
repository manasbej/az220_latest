using Microsoft.AspNetCore.Mvc;
namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class FileShareController : ControllerBase
{
    private readonly IAzureFileShareService _fileService;
    private readonly ILogger<FileShareController> _logger;

    public FileShareController(IAzureFileShareService fileService, ILogger<FileShareController> logger)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _logger = logger ?? throw new ArgumentNullException();
    }

    [HttpPost("CreateFile")]
    public async Task<IActionResult> PostAsync(string fileName, string text)
    {
        await _fileService.CreateFileWithTextAsync(fileName, text);
        return Ok("File Created");
    }
    [HttpPut("UpdateFile")]
    public async Task<IActionResult> UpdateAsync(string fileName, string text)
    {
        var result = await _fileService.UpdateFileWithTextAsync(fileName, text);
        return Ok(result);
    }
    [HttpDelete("Delete")]
    public async Task<IActionResult> DeleteAsync(string fileName)
    {
        var result = await _fileService.DeleteFileAsync(fileName);
        _logger.LogInformation(fileName, result);
        return Ok(result);
    }

    
}