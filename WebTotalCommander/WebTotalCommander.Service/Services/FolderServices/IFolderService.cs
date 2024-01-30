﻿using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FolderServices;

public interface IFolderService
{
    public Task<FolderGetAllModel> FolderGetAllAsync(string folder_path, string folder_name);
    public bool CreateFolder(FolderViewModel folderViewModel);
    public bool DeleteFolder(FolderViewModel folderViewModel);
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel);
}