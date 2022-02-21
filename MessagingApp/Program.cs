using MessagingApp.Extentions;
using MessagingApp.Interfaces;
using MessagingApp.Models;
using MessagingApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServices(builder);
builder.Services.AddAppServices(builder);
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x =>
{
    x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
