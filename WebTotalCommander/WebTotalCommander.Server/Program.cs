
using WebTotalCommander.Repository.Files;
using WebTotalCommander.Repository.Folders;
using WebTotalCommander.Server.ActionHelpers;
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
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //====================
        builder.Services.AddScoped<IFolderRepository, FolderRepository>();
        builder.Services.AddScoped<IFileRepository, FileRepository>();

        builder.Services.AddScoped<IFolderService, FolderService>();
        builder.Services.AddScoped<IFileService, FileService>();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
