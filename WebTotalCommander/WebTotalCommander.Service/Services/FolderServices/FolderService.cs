﻿using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Service.ViewModels;

namespace WebTotalCommander.Service.Services.FolderServices;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repository;

    public FolderService(IFolderRepository folderRepository)
    {
        this._repository = folderRepository;
    }
    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result=_repository.CreateFolder(folder);
        if(result)
        {
            return result;
        }
        else
        {
            throw new AlreadeExsistException("Folder alreade exsist!");
        }        
    }

    public bool DeleteFolder(FolderViewModel folderViewModel)
    {
        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result=_repository.DeleteFolder(folder);

        if(result)
        {
            return result;
        }
        else
        {
            throw new EntryNotFoundException("Folder not found!");
        }
    }

    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        FolderRename folderRename = new FolderRename()
        {
            FolderPath = folderRenameViewModel.FolderPath,
            FolderNewName = folderRenameViewModel.FolderNewName,
            FolderOldName = folderRenameViewModel.FolderOldName
        };
        bool result=_repository.RenameFolder(folderRename);
        if(result)
        {
            return result;
        }
        else
        {
            throw new EntryNotFoundException("Folder not found!");
        }
    }
}
