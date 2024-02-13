﻿using Microsoft.AspNetCore.Http;
using System.Text;
using WebTotalCommander.FileAccess.Models.File;

namespace WebTotalCommander.Repository.Files;

public class FileRepository : IFileRepository
{
    private string ROOTPATH = "DataFolder";

    public async Task<bool> CreateFile(FileModel file)
    {
        try
        {
            var path = Path.Combine(ROOTPATH, file.FilePath, file.FileSource.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await file.FileSource.CopyToAsync(stream);
            stream.Close();
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    public async Task<bool> DeleteFile(FileDeleteModel file)
    {
        var path = Path.Combine(ROOTPATH, file.FilePath, file.FileName);
        await Task.Run(() =>
        {
            File.Delete(path);
        });
        return true;
    }

    public async Task<MemoryStream> DownloadFileAsync(string filePath)
    {
        var path=Path.Combine(ROOTPATH, filePath);

        var memory = new MemoryStream();
        await using(var stream=new FileStream(path, FileMode.Open))
        {
           await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        return memory;
    }

    public async Task<bool> EditTextTxtFile(string filePath, IFormFile formFile)
    {
        try
        {
            var path = Path.Combine(ROOTPATH, filePath);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return true;
        }
        catch
        {
            return false;
        }
       
    }

    public async Task<MemoryStream> GetTxtFileAsync(string filePath)
    {
        var path = Path.Combine(ROOTPATH, filePath);

        var memory = new MemoryStream();
        await using (var stream = new FileStream(path, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        return memory;
    }
}
