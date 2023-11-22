using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using Ramboe.IS4.Data;

namespace Ramboe.IS4;

public class ProfileService : IProfileService
{
    readonly UserContext _db;

    public ProfileService(UserContext db)
    {
        _db = db;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var auth = context.Subject.Identity.IsAuthenticated;

        if (auth is not true)
        {
            return;
        }

        //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
        //where and subject was set to my user id.
        var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

        if (!string.IsNullOrEmpty(userId?.Value))
        {
            //get user from db (find user by user id)
            var user = _db.Users
                          .Include(u => u.Role)
                          .FirstOrDefault(u => u.Id.ToString() == userId.Value);

            // issue the claims for the user
            if (user is not null)
            {
                var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                context.IssuedClaims = claims.ToList();
            }
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
    }
}
