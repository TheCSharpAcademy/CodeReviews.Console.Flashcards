using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Domain;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    });

var app = builder.Build();


using var scope = app.Services.CreateScope();
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); 
}

var k = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
k.Add(new CardStack("English"));
k.SaveChanges();