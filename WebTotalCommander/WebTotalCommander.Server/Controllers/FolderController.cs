using Microsoft.AspNetCore.Mvc;
using WebTotalCommander.Service.Services.FolderServices;
using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Server.Controllers;

[Route("api/folder")]
[ApiController]
public class FolderController : ControllerBase
{
    private IFolderService _service;
    public FolderController(IFolderService fileService)
    {
        this._service = fileService;
    }

    [HttpGet]
    public IActionResult FolderGetAll([FromQuery] FolderGetAllQuery query)
    {
        var folderGetAllView = _service.FolderGetAll(query);
        return Ok(folderGetAllView);
    }

    [HttpGet("zip")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> FolderDownloadZipAsync(string folderName, string folderPath)
    {
        var result = await _service.DownloadFolderZipAsync(folderPath, folderName);
        return File(result.memoryStream, "application/zip", result.fileName);
    }

    [HttpPost]
    public IActionResult CreateFolder(FolderViewModel folderViewModel)
    {
        var result = _service.CreateFolder(folderViewModel);
        return Ok(new { result });
    }

    [HttpDelete]
    public IActionResult DeleteFolderAsync(FolderViewModel folderViewModel)
    {
        var result = _service.DeleteFolder(folderViewModel);
        return Ok(new { result });
    }

    [HttpPut]
    public IActionResult RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        var result = _service.RenameFolder(folderRenameViewModel);
        return Ok(new { result });
    }
}
