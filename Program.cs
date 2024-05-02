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
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Set the server port
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.Listen(IPAddress.Any, 5000); // Change 5000 to your desired port number
//});

// Add services to the container.
builder.Services.AddControllers();

//Adding cors 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

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
builder.Services.AddScoped<VoteBlogDTO>();
builder.Services.AddScoped<ValueMapper>();
builder.Services.AddScoped<FileUploaderHelper>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<BlogVoteRepository>();

// Add filters
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<AdminAuthFilter>();

var app = builder.Build();
app.UseCors("AllowOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Add this line to serve static files

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
