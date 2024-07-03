using EskroAfrica.PaymentService.API;
using Hangfire;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
IConfiguration config = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration.GetValue<string>("AppSettings:LogSettings:LogUrl"))
    .CreateLogger();

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