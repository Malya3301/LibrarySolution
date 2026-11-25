using Library.Server.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Library.Server.Services;

public class PasswordHasherService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, hashedPassword, password);
        
        return result is PasswordVerificationResult.Success 
            or PasswordVerificationResult.SuccessRehashNeeded;
    }
}