using Flashcards.UndercoverDev.DataConfig;

DatabaseManager dataConfig = new("FlashcardDB");

dataConfig.InitializeDatabase();
dataConfig.CreateStacksTables();
dataConfig.CreateFlashcardsTables();