using Flashcards.UndercoverDev.Controllers;
using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.UserInteraction;

UserConsole userConsole = new();
DatabaseManager dataConfig = new();
StackRepository stackRepository= new();
StackServices stackServices = new(userConsole, stackRepository);
FlashcardController flashcardController = new(userConsole, dataConfig, stackServices);

flashcardController.RunProgram();