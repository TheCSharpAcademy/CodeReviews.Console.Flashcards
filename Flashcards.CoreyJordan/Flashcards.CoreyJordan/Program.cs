using FlashcardsLibrary.Controller;

MainPage mainPage = new();

mainPage.DisplaySplashScreen();

bool exitFlashCard = false;
while (exitFlashCard == false)
{
    mainPage.DisplayMainMenu();
    exitFlashCard = mainPage.RunMainMenu();
}