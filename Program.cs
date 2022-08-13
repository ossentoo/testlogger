// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

IServiceProvider _serviceProvider;
ILogger<Program> _logger;


Console.WriteLine("Hello, World!");

var config = InitConfiguration();

IConfiguration InitConfiguration()
{
    var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    var services = new ServiceCollection();

    services.AddLogging(configure => configure.AddSerilog())
            .AddTransient<Program>();

    services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);

        // .AddLogging(builder => builder.AddSerilog(
        //     new LoggerConfiguration()
        //         .ReadFrom.Configuration(configuration)
        //         .CreateLogger()))
    _serviceProvider = services.BuildServiceProvider();

    _logger = _serviceProvider.GetService<ILogger<Program>>();
    _logger.LogInformation("Logger initialized!");

    var loggerFactory = _serviceProvider.GetService<ILoggerFactory>();
    _logger = loggerFactory.CreateLogger<Program>();
    return configuration;
}
