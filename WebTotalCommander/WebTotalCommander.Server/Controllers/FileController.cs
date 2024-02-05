using Microsoft.AspNetCore.Mvc;
using WebTotalCommander.Service.Services.FileServices;
using WebTotalCommander.Service.ViewModels.File;

namespace WebTotalCommander.Server.Controllers;

[Route("api/file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileService _service;

    public FileController(IFileService fileService)
    {
        this._service = fileService;
    }

    [HttpPost]
    [DisableRequestSizeLimit]

    public async Task<IActionResult> CreateFileAsync([FromForm] FileViewModel fileViewModel)
    {
        var result = await _service.CreateFile(fileViewModel);
        return Ok(new { result });
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteFileAsync(FileDeleteViewModel fileDeleteView)
    {
        var result = await _service.DeleteFile(fileDeleteView);
        return Ok(new { result });
    }
    [HttpGet]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> DownloadFileAsync([FromQuery] string filePath)
    {
        var result = await _service.DownloadFileAsync(filePath);
        return File(result.memoryStream, "application/octet-stream", result.filePath);
    }

}
