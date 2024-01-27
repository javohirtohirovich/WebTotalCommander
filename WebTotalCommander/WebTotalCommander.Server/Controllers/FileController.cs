using Microsoft.AspNetCore.Mvc;
using WebTotalCommander.Service.Services.FileServices;
using WebTotalCommander.Service.ViewModels;

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
    public async Task<IActionResult> CreateFile(FileViewModel fileViewModel)
    {
        var result = await _service.CreateFile(fileViewModel);

        return Ok(new { result });
    }
    [HttpDelete]
    public async Task<IActionResult> DeleteFile(FileDeleteViewModel fileDeleteView)
    {
        var result=await _service.DeleteFile(fileDeleteView);
        return Ok(new { result });
    }
    
}
