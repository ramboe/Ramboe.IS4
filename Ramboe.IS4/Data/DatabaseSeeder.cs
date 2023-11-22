using Ramboe.IS4.Data.Models;

namespace Ramboe.IS4.Data;

public class DatabaseSeeder
{
    public static void Seed(UserContext context)
    {
        if (!context.Roles.Any())
        {
            // Seed roles
            context.Roles.AddRange(
            new RoleEntity {Id = Guid.NewGuid(), Name = "Admin"},
            new RoleEntity {Id = Guid.NewGuid(), Name = "User"}
            );

            context.SaveChanges();
        }

        var password = "kaese";

        if (!context.Users.Any())
        {
            // Seed users
            var adminRole = context.Roles.First(r => r.Name == "Admin");
            var userRole = context.Roles.First(r => r.Name == "User");

            context.Users.AddRange(
            new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PasswordHashed = password.CalculateMD5Hash(),
                DateOfBirth = new DateTime(1990, 1, 1),
                Role = adminRole
            },
            new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PasswordHashed = password.CalculateMD5Hash(),
                DateOfBirth = new DateTime(1995, 5, 5),
                Role = userRole
            }
            );
        }

        context.SaveChanges();
    }
}
