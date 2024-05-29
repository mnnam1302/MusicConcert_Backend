using Application.Abstractions;
using Firebase.Auth;
using Firebase.Storage;
using Infrastructure.DependencyInjection.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.UploadImage;

public class FirebaseService : IFirebaseService
{
    private readonly FirebaseStorage _firebaseStorage;
    private readonly FirebaseAuthProvider _authProvider;
    private readonly FireBaseOptions _firebaseOptions;

    public FirebaseService(FirebaseStorage firebaseStorage, FirebaseAuthProvider authProvider, IConfiguration configuration)
    {
        _firebaseStorage = firebaseStorage;
        _authProvider = authProvider;
        _firebaseOptions = new FireBaseOptions();
        configuration.GetSection(nameof(FireBaseOptions)).Bind(_firebaseOptions);
    }

    public async Task<bool> Authentication()
    {
        var result = await _authProvider.SignInWithEmailAndPasswordAsync(
            _firebaseOptions.AuthEmail,
            _firebaseOptions.AuthPassword);

        return result != null;
    }

    public async Task<string> UploadImage(IFormFile file)
    {
        var stream = file.OpenReadStream();

        var name = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);

        string fileName = name + DateTime.Now.Ticks.ToString() + extension;

        var imageUrl = await _firebaseStorage
                .Child("images")
                .Child(fileName)
                .PutAsync(stream);

        return imageUrl;
    }
}