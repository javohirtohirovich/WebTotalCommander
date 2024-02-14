﻿using WebTotalCommander.Core.Errors;
using WebTotalCommander.FileAccess.Models.Folder;
using WebTotalCommander.FileAccess.Utils;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.Common.Service;
using WebTotalCommander.Service.ViewModels.Common;
using WebTotalCommander.Service.ViewModels.Folder;

namespace WebTotalCommander.Service.Services.FolderServices;

public class FolderService : IFolderService
{
    private readonly IFolderRepository _repository;
    private readonly ISorter _sorter;
    private readonly string _mainFolderName = "DataFolder";

    //Konstruktor
    public FolderService(IFolderRepository folderRepository, ISorter sorter)
    {
        this._repository = folderRepository;
        this._sorter = sorter;
    }

    //Function Create Folder (API)
    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (Directory.Exists(path)) { throw new AlreadeExsistException("Folder alreade exsist!"); }

        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result = _repository.CreateFolder(folder);

        return result;
    }

    //Function Delete Folder (API)
    public bool DeleteFolder(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (!Directory.Exists(path)) { throw new EntryNotFoundException("Folder not found!"); }

        Folder folder = new Folder()
        {
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        bool result = _repository.DeleteFolder(folder);

        return result;
    }

    //Function Download Folder Zip (API)
    public async Task<(MemoryStream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName)
    {
        var path = Path.Combine(_mainFolderName, folderPath, folderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var memory = await _repository.DownloadFolderZipAsync(folderPath, folderName);

        return (memory, folderName);
    }

    //Function GetAll Folder (API)
    public async Task<FolderGetAllViewModel> FolderGetAllAsync(FolderGetAllQuery query)
    {
        var path = Path.Combine(_mainFolderName, query.Path);

        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var folderGetAll = await _repository.GetAllFolder(query.Path);

        var result = new FolderGetAllViewModel();

        //Add folders in result
        for (int i = 0; i < folderGetAll.FolderNames.Count; i++)
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

        //Add files in result
        for (int i = 0; i < folderGetAll.Files.Count; i++)
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

        Paginator paginator = new Paginator();
        var paginationMetaData = paginator.Paginate(result.FolderFile.Count, new PaginationParams(query.Offset, query.Limit));

        if (query.SortDir == "desc")
        {
            result.FolderFile = _sorter.SortDesc(query, result.FolderFile);
        }
        else if(query.SortDir=="asc")
        {
            result.FolderFile = _sorter.SortAsc(query, result.FolderFile);
        }

        if (query.Filter != null)
        {
            foreach (var item in query.Filter.Filters)
            {
                var isName = item.Filters.Any(x => x.Field == "name");
                if (isName)
                {
                    var containsFilter = item.Filters.FirstOrDefault(x => x.Operator == "contains");
                    if (containsFilter != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.Name.Contains(containsFilter.Value)).ToList();
                    }
                }
            }
        }

        result.PaginationMetaData = paginationMetaData;
        result.FolderFile = result.FolderFile.Skip(query.Offset).Take(query.Limit).ToList();

        return result;
    }

    //Function Rename folder (API)
    public bool RenameFolder(FolderRenameViewModel folderRenameViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderRenameViewModel.FolderPath, folderRenameViewModel.FolderOldName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        FolderRename folderRename = new FolderRename()
        {
            FolderPath = folderRenameViewModel.FolderPath,
            FolderNewName = folderRenameViewModel.FolderNewName,
            FolderOldName = folderRenameViewModel.FolderOldName
        };
        var result = _repository.RenameFolder(folderRename);

        return result;
    }

}
