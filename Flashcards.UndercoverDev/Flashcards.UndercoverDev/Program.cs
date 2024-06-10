using Flashcards.UndercoverDev.Controllers;
using Flashcards.UndercoverDev.DataConfig;
using Flashcards.UndercoverDev.Repository;
using Flashcards.UndercoverDev.Repository.StudySessions;
using Flashcards.UndercoverDev.Services;
using Flashcards.UndercoverDev.Services.Session;
using Flashcards.UndercoverDev.UserInteraction;

UserConsole userConsole = new();
DatabaseManager dataConfig = new();
StackRepository stackRepository= new();
FlashcardRepository flashcardRepository = new();
SessionRepository sessionRepository = new();
StackServices stackServices = new(userConsole, stackRepository, flashcardRepository);
FlashcardServices flashcardServices = new(userConsole, flashcardRepository, stackRepository, stackServices);
SessionServices sessionServices = new(userConsole, sessionRepository, stackRepository, flashcardRepository);
FlashcardController flashcardController = new(userConsole, dataConfig, stackServices, flashcardServices, sessionServices);

flashcardController.RunProgram();