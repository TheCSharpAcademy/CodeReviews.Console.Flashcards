using FlashcardStack.AshtonLeeSeloka.Controllers;
using FlashcardStack.AshtonLeeSeloka.Services;

StartUpDataService _StartUpDataBaseService = new StartUpDataService();
HomeController _HomeController = new HomeController();

_StartUpDataBaseService.StartUpDB();
_HomeController.Start();
