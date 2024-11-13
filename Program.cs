using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Flashcards.TwilightSaw.Controller;
using Flashcards.TwilightSaw.Factory;
using Flashcards.TwilightSaw.View;

var app = HostFactory.CreateDbHost(args);

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
context.Database.Migrate(); 

Menu menu = new Menu();

menu.MainMenu(context);
