using Microsoft.AspNetCore.Mvc;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.Service.Services.FolderServices;
using WebTotalCommander.Service.ViewModels;

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
    public async Task<IActionResult> GetAllAsync([FromQuery]string folder_path="",[FromQuery] string folder_name = "")
    {
        FolderGetAllModel folderGetAll=await _service.FolderGetAllAsync(folder_path,folder_name);
        return Ok(folderGetAll );
    }

    [HttpPost]
    public IActionResult CreateFolder(FolderViewModel folderViewModel)
    {
        bool result=_service.CreateFolder(folderViewModel);
        return Ok(new { result });
    }
    [HttpDelete]
    public IActionResult DeleteFolder(FolderViewModel folderViewModel)
    {
        bool result=_service.DeleteFolder(folderViewModel);
        return Ok(new { result });
    }
    [HttpPut]
    public IActionResult RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        bool result=_service.RenameFolder(folderRenameViewModel);
        return Ok(new { result });
    }
}
