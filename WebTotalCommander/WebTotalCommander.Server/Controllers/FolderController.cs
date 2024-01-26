using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("create")]
    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        bool result=_service.CreateFolder(folderViewModel);
        return result;
    }
    [HttpDelete("delete")]
    public bool DeleteFolder(FolderViewModel folderViewModel)
    {
        bool result=_service.DeleteFolder(folderViewModel);
        return result;
    }
}
