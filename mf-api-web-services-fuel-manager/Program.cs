using mf_api_web_services_fuel_manager.Models;
using Microsoft.EntityFrameworkCore;
using System.Text
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
.object value = AddJsonOptions(X =); x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();