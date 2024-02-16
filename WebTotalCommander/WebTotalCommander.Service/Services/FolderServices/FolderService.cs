﻿using Microsoft.Extensions.Configuration;
using WebTotalCommander.Core.Errors;
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
    private readonly string _mainFolderName;

    public FolderService(IFolderRepository folderRepository, ISorter sorter, IConfiguration config)
    {
        this._repository = folderRepository;
        this._sorter = sorter;
        this._mainFolderName = config["MainFolderName"];
    }

    public bool CreateFolder(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (Directory.Exists(path))
        {
            throw new AlreadeExsistException("Folder alreade exsist!");
        }

        Folder folder = new Folder()
        {
            FolderMainName = _mainFolderName,
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        var result = _repository.CreateFolder(folder);

        return result;
    }

    public async Task<bool> DeleteFolderAsync(FolderViewModel folderViewModel)
    {
        var path = Path.Combine(_mainFolderName, folderViewModel.FolderPath, folderViewModel.FolderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        Folder folder = new Folder()
        {
            FolderMainName = _mainFolderName,
            FolderName = folderViewModel.FolderName,
            FolderPath = folderViewModel.FolderPath,
        };

        var result = await _repository.DeleteFolderAsync(folder);

        return result;
    }

    public async Task<(MemoryStream memoryStream, string fileName)> DownloadFolderZipAsync(string folderPath, string folderName)
    {
        var path = Path.Combine(_mainFolderName, folderPath, folderName);
        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var memory = await _repository.DownloadFolderZipAsync(path);

        return (memory, folderName);
    }

    public async Task<FolderGetAllViewModel> FolderGetAllAsync(FolderGetAllQuery query)
    {
        var path = Path.Combine(_mainFolderName, query.Path);

        if (!Directory.Exists(path))
        {
            throw new EntryNotFoundException("Folder not found!");
        }

        var folderGetAll = await _repository.GetAllFolderAsync(path);

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

        var paginator = new Paginator();
        var paginationMetaData = paginator.Paginate(result.FolderFile.Count, new PaginationParams(query.Offset, query.Limit));

        if (query.SortDir == "desc")
        {
            result.FolderFile = _sorter.SortDesc(query, result.FolderFile);
        }
        else if (query.SortDir == "asc")
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
                var isExtension = item.Filters.Any(x => x.Field == "extension");
                if (isExtension)
                {
                    var containsFilter = item.Filters.FirstOrDefault(x => x.Operator == "contains");
                    if (containsFilter != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.Extension.Contains(containsFilter.Value)).ToList();
                    }
                }

                var isCreateDate = item.Filters.Any(x => x.Field == "createdDate");
                if (isCreateDate)
                {
                    var containsFilterEq = item.Filters.FirstOrDefault(x => x.Operator == "eq");
                    if (containsFilterEq != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.CreatedDate == DateTime.Parse(containsFilterEq.Value)).ToList();
                    }
                    var containsFilterGte = item.Filters.FirstOrDefault(x => x.Operator == "gte");
                    if (containsFilterGte != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.CreatedDate >= DateTime.Parse(containsFilterGte.Value)).ToList();
                    }
                    var containsFilterLte = item.Filters.FirstOrDefault(x => x.Operator == "lte");
                    if (containsFilterLte != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.CreatedDate <= DateTime.Parse(containsFilterLte.Value)).ToList();
                    }
                    var containsFilterGt = item.Filters.FirstOrDefault(x => x.Operator == "gt");
                    if (containsFilterGt != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.CreatedDate > DateTime.Parse(containsFilterGt.Value)).ToList();
                    }
                    var containsFilterLt = item.Filters.FirstOrDefault(x => x.Operator == "lt");
                    if (containsFilterLt != null)
                    {
                        result.FolderFile = result.FolderFile.Where(x => x.CreatedDate < DateTime.Parse(containsFilterLt.Value)).ToList();
                    }
                }
            }
        }

        result.PaginationMetaData = paginationMetaData;
        result.FolderFile = result.FolderFile.Skip(query.Offset).Take(query.Limit).ToList();

        return result;
    }

    public async Task<bool> RenameFolderAsync(FolderRenameViewModel folderRenameViewModel)
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

        var result = await _repository.RenameFolderAsync(folderRename);
        return result;
    }

}
