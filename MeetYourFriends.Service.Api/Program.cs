using Serilog;
using System.Diagnostics;

namespace BookEverything_Api_Service;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configurationRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("serilog.json", optional: false, reloadOnChange: true)
            .Build();

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", true, true)
                    .AddEnvironmentVariables();
            })
            .UseSerilog((hostingContext, services, configuration) =>
            {
                configuration
                    .ReadFrom.Services(services)
                    .ReadFrom.Configuration(configurationRoot)
                    .ReadFrom.Configuration(hostingContext.Configuration);

                configuration.Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached);
            })
            .ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseIISIntegration().UseStartup<Startup>();
            }).Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configurationRoot)
            .CreateLogger();

        Log.Information("start");

        await host.RunAsync();
    }
}

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookEverything-Api-Service v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}