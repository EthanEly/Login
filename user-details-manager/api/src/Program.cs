using Microsoft.EntityFrameworkCore;
using User.Interfaces.Services;
using User.Interfaces.Respositories;
using User.Respositories.DatabaseContext;
using User.Services.UserService;
using User.Respositories.UserRepository;
using User.Models.UserRegistration;
using User.Models.UserLogin;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' was not found in the configuration.");
}
builder.Services.AddDbContext<UserDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/register", async (IUserService userService, UserRegistration registration) =>
{
    try
    {
        await userService.Register(registration);
        return Results.Ok("User registered successfully");
    }
    catch (InvalidOperationException ex)
    {
        return Results.Conflict(ex.Message);
    }
});

app.MapPost("/login", async (IUserService userService, UserLogin login) =>
{
    var isLoggedIn = await userService.Login(login);
    return isLoggedIn ? Results.Ok("Login successful") : Results.Unauthorized();
});

app.Run();
