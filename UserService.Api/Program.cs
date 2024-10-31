using UserService.Api.Middlewares;
using UserService.Infrastructure;
using UserService.Infrastructure.DependencyInjection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add application services
builder.Services.ConnectToDatabase(builder.Configuration, options => options.UseSQLServer = true);
builder.Services.AddIdentityAndJwtRegistration(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddObservability(builder.Configuration);


var app = builder.Build();



// Run migrations at startup
var migrator = new DatabaseMigrator(app.Services);
migrator.Migrate();


app.UseMiddleware<ExceptionMiddleware>();


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



