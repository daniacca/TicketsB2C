var builder = WebApplication.CreateBuilder(args);

// Register Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Build application
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Run
app.Run();
