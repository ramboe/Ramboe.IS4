using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Ramboe.IS4.Data;
using Ramboe.IS4.Data.Models;

namespace Ramboe.IS4;

/// <summary>
/// Executed if token_endpoint ("https://localhost:7215/connect/token") is hit with 'grant_type' 'password' 
/// </summary>
public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    readonly UserContext _db;

    public ResourceOwnerPasswordValidator(UserContext db)
    {
        _db = db;
    }

    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        #region
        var foundUser = _db.Users.FirstOrDefault(u => u.Email.ToLower() == context.UserName.ToLower());

        if (foundUser is null)
        {
            return returnInvalidGrant($"user {context.UserName} not found");
        }

        var hashed = context.Password.CalculateMD5Hash();

        if (foundUser.PasswordHashed != hashed)
        {
            return returnInvalidGrant("Password wrong");
        }
        #endregion

        //custom sql logic here
        context.Result = new GrantValidationResult(
        foundUser.Id.ToString(),
        GrantType.ResourceOwnerPassword,
        new[]
        {
            new Claim(ClaimTypes.Name, context.UserName),
        });

        return Task.CompletedTask;

        Task returnInvalidGrant(string errorDescription)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, errorDescription);

            return Task.CompletedTask;
        }
    }

    public static Claim[] GetUserClaims(UserEntity user)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };
    }
}
