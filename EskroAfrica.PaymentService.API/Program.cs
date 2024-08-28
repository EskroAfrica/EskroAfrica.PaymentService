using Confluent.Kafka;
using EskroAfrica.PaymentService.API;
using EskroAfrica.PaymentService.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = builder.Configuration;

bool writeToSeq = builder.Configuration.GetValue<bool>("AppSettings:LogSettings:WriteToSeq");
bool writeToFile = builder.Configuration.GetValue<bool>("AppSettings:LogSettings:WriteToFile");

if (writeToSeq)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.Seq(builder.Configuration.GetValue<string>("AppSettings:LogSettings:LogUrl"))
        .CreateLogger();
}
else if (writeToFile)
{
    Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("logs/log.txt",
            rollingInterval: RollingInterval.Day,
            rollOnFileSizeLimit: true)
        .CreateLogger();
}

Log.Information("Starting up PaymentService");

try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddConfigurations(config);

    builder.Services.AddCors();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    if (builder.Configuration.GetValue<bool>("AppSettings:EnableMigration"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentServiceDbContext>();
            dbContext.Database.Migrate();
        }
    }

    app.UseCors(options =>
    {
        options.AllowAnyOrigin();
    });

    app.UseHttpsRedirection();

    app.UseHangfireDashboard();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers().RequireAuthorization();

    app.Run();
}
catch (Exception e)
{
    Log.Fatal($"Failed to start PaymentService - {e.Message}");
}
finally
{
    Log.Information("Closing PaymentService");
    Log.CloseAndFlush();
}