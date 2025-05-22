
using Scalar.AspNetCore;
using TicketsB2C.Services;

var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddServices(builder.Configuration);

// Build application
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run
app.Run();
