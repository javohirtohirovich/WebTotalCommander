﻿
using WebTotalCommander.FileAccess.Models;

namespace WebTotalCommander.Repository.Folders;

public interface IFolderRepository
{
    public bool CreateFolder(Folder folder);
    public bool DeleteFolder(Folder folder);
    public bool RenameFolder(FolderRename folderRename);

}
