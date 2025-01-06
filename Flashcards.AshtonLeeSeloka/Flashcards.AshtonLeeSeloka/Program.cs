using Flashcards.AshtonLeeSeloka.Controllers;
using Flashcards.AshtonLeeSeloka.Services;

StartUpDataService _StartUpDataBaseService = new StartUpDataService();
HomeController _HomeController = new HomeController();

_StartUpDataBaseService.StartUpDB();
_HomeController.Start();
