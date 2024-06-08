using Flashcards.UndercoverDev.Controllers;
using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.UserInteraction;

UserConsole userConsole = new();
DatabaseManager dataConfig = new();
StackRepository stackRepository= new();
FlashcardRepository flashcardRepository = new();
StackServices stackServices = new(userConsole, stackRepository, flashcardRepository);
FlashcardServices flashcardServices = new(userConsole, flashcardRepository, stackRepository, stackServices);
FlashcardController flashcardController = new(userConsole, dataConfig, stackServices, flashcardServices);

flashcardController.RunProgram();