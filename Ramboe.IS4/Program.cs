using Microsoft.EntityFrameworkCore;
using Ramboe.IS4;
using Ramboe.IS4.Data;

var builder = WebApplication.CreateBuilder(args);

var issuer = builder.Configuration.GetSection("Is4:IssuerUri").Get<string>();

builder.Services.AddIdentityServer(options => {
           options.IssuerUri = issuer;
       })
       .AddInMemoryApiResources(Configuration.GetApis())
       .AddInMemoryClients(Configuration.GetClients())
       .AddInMemoryApiScopes(Configuration.GetScopes())
       .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()//needed for password validation
       .AddProfileService<ProfileService>()

       // .AddSigningCredential()
       .AddDeveloperSigningCredential();//we need a key to sign the token with

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins,
    policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

#region postgres
var sqlConnectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql(sqlConnectionString));
#endregion

var app = builder.Build();

#region seeding
using var scope = app.Services.CreateScope();

var serviceProvider = scope.ServiceProvider;
var userContext = serviceProvider.GetRequiredService<UserContext>();
userContext.Database.EnsureCreated();

DatabaseSeeder.Seed(userContext);
#endregion

app.UseCors(MyAllowSpecificOrigins);

app.UseIdentityServer();

app.MapGet("/", () => "Authority running, issuer: " + issuer);

app.Run();
