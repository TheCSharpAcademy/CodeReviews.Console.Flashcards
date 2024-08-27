using FlashCards;
using Microsoft.Extensions.Configuration;
using Serilog;

var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var console = new SpectreConsole();

LocalDbManager.CreateDatabase("FlashCardsDB");
var dapper = new DapperHelper(config, logger);
dapper.InitializeDatabase();

var controller = new FlashCardsController(logger, config, console, dapper);
controller.Run();