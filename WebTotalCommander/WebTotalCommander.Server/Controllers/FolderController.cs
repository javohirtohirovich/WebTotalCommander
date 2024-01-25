using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTotalCommander.Service.Services.FolderServices;
using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Server.Controllers
{
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
            _service.CreateFolder(folderViewModel);
            return true;
        }
    }
}
