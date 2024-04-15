using GroupCoursework.DatabaseConfig;
using GroupCoursework.Filters;
using GroupCoursework.Repositories;
using GroupCoursework.Repository;
using GroupCoursework.Service;
using Microsoft.EntityFrameworkCore;
using System.Net;
using GroupCoursework.Models;
using GroupCoursework.DTO;
using GroupCoursework.Utils;
using Microsoft.AspNetCore.Builder;

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

builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostBlogDTO>();
builder.Services.AddScoped<ValueMapper>();
builder.Services.AddScoped<FileUploaderHelper>();
builder.Services.AddScoped<UserRepository>();

// Add filters
builder.Services.AddScoped<AuthFilter>();

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
