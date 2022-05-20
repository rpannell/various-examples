// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Serilog;

Console.WriteLine("Hello, World!");

//create a configuration to configure serilog via the appsetting.json
var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

//create the logger
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.File("Logs/Info-.log", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.FromSeconds(10))
    .CreateLogger();

//create a log entry
Log.Logger.Information("New Info");

//
Console.WriteLine("File Should be Written in bin/Debug/net6.0/Logs/Info");