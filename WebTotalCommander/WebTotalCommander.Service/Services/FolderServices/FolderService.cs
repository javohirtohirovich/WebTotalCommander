﻿using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Repository.Settings;
using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.Common.Service;
using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repository;
    private readonly ISorter _sorter;
    private readonly IFilter _filter;
    private readonly string _mainFolderName;

    public FolderService(IFolderRepository folderRepository, ISorter sorter, IFilter filter, FolderSettings folderSettings)
    {
        this._repository = folderRepository;
        this._sorter = sorter;
        this._filter = filter;
        this._mainFolderName = folderSettings.MainFolderName;
    }

    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (Directory.Exists(path))
        {
            throw new AlreadeExsistException("Folder alreade exsist!");
        }

        var folder = new Folder()
        {
            FolderMainName = _mainFolderName,
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        var result = _repository.CreateFolder(folder);

        return result;
    }

    public bool DeleteFolder(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var folder = new Folder()
        {
            FolderMainName = _mainFolderName,
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        var result = _repository.DeleteFolder(folder);

        return result;
    }

    public async Task<(Stream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName)
    {
        if (folderPath == null) { folderPath = ""; }
        var path = Path.Combine(_mainFolderName, folderPath, folderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var memory = await _repository.DownloadFolderZipAsync(path);

        return (memory, folderName);
    }

    public FolderGetAllViewModel FolderGetAll(FolderGetAllQuery query)
    {
        if (query.Path == null) { query.Path = ""; }
        var path = Path.Combine(_mainFolderName, query.Path);

        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var folderGetAll = _repository.GetAllFolder(path);

        var result = new FolderGetAllViewModel();

        for (var i = 0; i < folderGetAll.FolderNames.Count; i++)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderGetAll.FolderNames[i]);
            FolderFileViewModel folderView = new FolderFileViewModel()
            {
                Name = directoryInfo.Name,
                Path = folderGetAll.FolderNames[i],
                Extension = "folder",
                CreatedDate = directoryInfo.CreationTime
            };
            result.FolderFile.Add(folderView);
        }

        for (var i = 0; i < folderGetAll.Files.Count; i++)
        {
            FileInfo fileInfo = new FileInfo(folderGetAll.Files[i]);
            FolderFileViewModel fileView = new FolderFileViewModel()
            {
                Name = fileInfo.Name,
                Extension = fileInfo.Extension,
                Path = folderGetAll.Files[i],
                CreatedDate = fileInfo.CreationTime,
            };
            result.FolderFile.Add(fileView);
        }

        if (query.Filter != null)
        {
            result.FolderFile = _filter.FilterFolder(query.Filter, result.FolderFile);
        }

        if (query.SortDir == "desc")
        {
            result.FolderFile = _sorter.SortDesc(query, result.FolderFile);
        }
        else if (query.SortDir == "asc")
        {
            result.FolderFile = _sorter.SortAsc(query, result.FolderFile);
        }

        var paginator = new Paginator();
        var paginationMetaData = paginator.Paginate(result.FolderFile.Count, new PaginationParams(query.Offset, query.Limit));
        result.PaginationMetaData = paginationMetaData;
        result.FolderFile = result.FolderFile.Skip(query.Offset).Take(query.Limit).ToList();

        return result;
    }

    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderRenameViewModel.FolderPath, folderRenameViewModel.FolderOldName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        FolderRename folderRename = new FolderRename()
        {
            MainFolderName = _mainFolderName,
            FolderPath = folderRenameViewModel.FolderPath,
            FolderNewName = folderRenameViewModel.FolderNewName,
            FolderOldName = folderRenameViewModel.FolderOldName
        };

        var result = _repository.RenameFolder(folderRename);
        return result;
    }

}
