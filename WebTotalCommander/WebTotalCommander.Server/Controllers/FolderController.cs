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
    public async Task<IActionResult> FolderGetAllAsync(string folder_path = "", string folder_name = "")
    {
        IList<FolderGetAllViewModel> folderGetAllView = await _service.FolderGetAllAsync(folder_path, folder_name);
        return Ok(folderGetAllView);
    }

    [HttpPost]
    public IActionResult CreateFolder(FolderViewModel folderViewModel)
    {
        bool result = _service.CreateFolder(folderViewModel);
        return Ok(new { result });
    }
    [HttpDelete]
    public IActionResult DeleteFolder(FolderViewModel folderViewModel)
    {
        bool result = _service.DeleteFolder(folderViewModel);
        return Ok(new { result });
    }
    [HttpPut]
    public IActionResult RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        bool result = _service.RenameFolder(folderRenameViewModel);
        return Ok(new { result });
    }
}
