using Hello.Api.Configuration;
using Hello.Api.Models;  // Ensure this is imported
using Hello.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind BackgroundServiceSettings to appsettings.json configuration
builder.Services.Configure<BackgroundServiceSettings>(
    builder.Configuration.GetSection("BackgroundServiceSettings"));

// Register the shared product list as a singleton
builder.Services.AddSingleton<List<Product>>();

// Register the background services
builder.Services.AddHostedService<ProductGenerationService>();
builder.Services.AddHostedService<ProductRemovalService>();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
