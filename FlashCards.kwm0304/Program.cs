﻿/*
This is an application where the users will create Stacks of Flashcards.
 You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.

 Stacks should have an unique name.

 Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.

 You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.

 When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. 
 If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.

 After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.

 The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.

 The project should contain a call to the study table so the users can see all their study sessions. 
 This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.

 Challenges - Create Report function where you see average uses per month and another one with average score per month
*/
using FlashCards.kwm0304.Config;

namespace FlashCards.kwm0304;

public class Program
{
  public static async Task Main(string[] args)
  {
    var connString = AppConfiguration.GetConnectionString("DefaultConnection");
    var dbConfig = new DatabaseConfiguration(connString);
    var loop = new SessionLoop(dbConfig);
    await loop.OnStart();
  }
}