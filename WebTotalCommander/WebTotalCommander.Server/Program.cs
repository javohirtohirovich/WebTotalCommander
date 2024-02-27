
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Repository.Settings;
using WebTotalCommander.Server.ActionHelpers;
using WebTotalCommander.Server.Configuration;
using WebTotalCommander.Service.Common.Interface;
using WebTotalCommander.Service.Common.Service;
using WebTotalCommander.Service.Services.FileServices;
using WebTotalCommander.Service.Services.FolderServices;

namespace WebTotalCommander.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>());

        var mainFolderPath = builder.Configuration["MainFolderName"];
        var settings = new FolderSettings(mainFolderPath);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureCORSPolice();

        //Configure big file upload
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 1024 * 1024 * 1024;
        });
        //====================
        builder.Services.AddSingleton<FolderSettings>(settings);

        builder.Services.AddScoped<IFolderRepository, FolderRepository>();
        builder.Services.AddScoped<IFileRepository, FileRepository>();

        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<IFileService, FileService>();

        builder.Services.AddScoped<ISorter, Sorter>();
        builder.Services.AddScoped<IPaginator, Paginator>();
        builder.Services.AddScoped<IFilter, Filter>();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowSpecificOrigin");
        app.UseHttpsRedirection();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "DataFolder")),
            RequestPath = "/DataFolder"
        });
        app.UseAuthorization();
        app.MapControllers();
        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
