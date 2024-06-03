using Flashcards.UndercoverDev.Controllers;
using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.UserInteraction;

UserConsole userConsole = new();
DatabaseManager dataConfig = new("FlashcardDB");
StackServices stackServices = new(userConsole);
FlashcardController flashcardController = new(userConsole, dataConfig, stackServices);

flashcardController.RunProgram();