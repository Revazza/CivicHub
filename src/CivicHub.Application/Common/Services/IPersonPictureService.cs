using Microsoft.AspNetCore.Http;

namespace CivicHub.Application.Common.Services;

public interface IPersonPictureService
{
    Task<string> SaveAsync(long personId, IFormFile file);
}