using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure;
using UserService.Infrastructure.Data;
using UserService.Infrastructure.DependencyInjection;
using UserService.Infrastructure.Identity;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddIdentityAndJwtRegistration(builder.Configuration);

// Add application services
builder.Services.AddInfrastructureServices();
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddObservability(builder.Configuration);


var app = builder.Build();



// Run migrations at startup
var migrator = new DatabaseMigrator(app.Services);
migrator.Migrate();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseHttpsRedirection();
}


app.UseAuthorization();

app.MapControllers();

app.Run();



