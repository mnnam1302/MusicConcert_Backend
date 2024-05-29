
using Microsoft.AspNetCore.Http;

namespace Application.Abstractions;

public interface IFirebaseService
{
    Task<bool> Authentication();

    Task<string> UploadImage(IFormFile file);
}