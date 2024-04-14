using GroupCoursework.DatabaseConfig;
using GroupCoursework.Repositories;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Set the server port
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(IPAddress.Any, 5000); // Change 5000 to your desired port number
//});

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context
builder.Services.AddDbContext<AppDatabaseContext>();

// Add repositories and services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Apply database migrations
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDatabaseContext>();
    context.Database.Migrate();
}

app.MapControllers();

app.Run();
