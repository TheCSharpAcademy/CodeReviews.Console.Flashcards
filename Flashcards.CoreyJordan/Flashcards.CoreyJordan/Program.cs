﻿using FlashcardsLibrary.Controller;

MainPage mainPage = new();
mainPage.DisplaySplashScreen();

bool exitFlashCard = false;
while (exitFlashCard == false)
{
    exitFlashCard = mainPage.RunMainMenu();
}