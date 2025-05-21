using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            Console.WriteLine($"Connection String: {context.Configuration.GetConnectionString("DefaultConnection")}");

            services.AddDbContext<TicketsDbContext>(options =>
                options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
        });

var host = CreateHostBuilder(args).Build();
var retryCount = 0;

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketsDbContext>();
    Console.WriteLine("Running DB migrations");

    while (retryCount < 10)
    {
        try
        {
            await db.Database.MigrateAsync();
            break;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error during migration: {ex.Message}");
            retryCount++;
            await Task.Delay(2000 * retryCount);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            break;
        }
    }
}

Console.WriteLine("DB Migration Finish");
// host.Run();