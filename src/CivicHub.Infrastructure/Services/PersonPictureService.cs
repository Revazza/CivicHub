using CivicHub.Application.Common.Services;
using Microsoft.AspNetCore.Http;

namespace CivicHub.Infrastructure.Services;

public class PersonPictureService : IPersonPictureService
{
    private const string FolderName = "Pictures";

    public async Task<string> SaveAsync(long personId, IFormFile file)
    {
        var folderPath = GetPictureFolderPath();
        CreatePictureFolderIfDoesntExist(folderPath);

        var fullPath = GetFullPath(personId, folderPath, file);

        await AddPictureAsync(fullPath, file);

        return fullPath;
    }

    private static string GetPictureFolderPath() => Path.Combine(Directory.GetCurrentDirectory(), FolderName);

    private static void CreatePictureFolderIfDoesntExist(string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath!);
        }
    }

    private static string CreateFileName(long personId, string extension) => $"{personId}_Picture{extension}";

    private static async Task AddPictureAsync(string fullPath, IFormFile file)
    {
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    private static string GetFullPath(long personId, string folderPath, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = CreateFileName(personId, extension);
        return Path.Combine(folderPath, fileName);
    }
}