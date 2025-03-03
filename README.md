# Flashcards Project

  First time using SQL Server instead of SQLite, intro to Entity Framework,
  and 2nd project after inital tutorial to display using .NET MAUI.

  This app uses linked tables to display Stacks, Flashcards, Study Sessions,
  and let the user study a given stack.

## Given Requirements

- [x] You'll need two different tables for stacks and flashcards. The tables
      should be linked by a foreign key.

- [x] Stacks should have an unique name.
      
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the
      same should happen with the flashcard.
      
- [x] You should use DTOs to show the flashcards to the user without the Id of
      the stack it belongs to.
      
- [x] When showing a stack to the user, the flashcard Ids should always start
      with 1 without gaps between them. If you have 10 cards and number 5 is
      deleted, the table should show Ids from 1 to 9.
      
- [x] After creating the flashcards functionalities, create a "Study Session"
      area, where the users will study the stacks. All study sessions should
      be stored, with date and score.
      
- [x] The study and stack tables should be linked. If a stack is deleted, it's
       study sessions should be deleted.
      
- [x] The project should contain a call to the study table so the users can
      see all their study sessions. This table receives insert calls upon each
       study session, but there shouldn't be update and delete calls to it.

## Features

- SQL Server Database connection to localdb
  
- Windows-only .NET MAUI GUI display
  
  - User can navigate by clicking on buttons.
    
  - 2-way data binding between UI and VM for neat display.
    
- CRUD db operations for stacks & flashcards
  
  - Cascade delete when stack is deleted, linked flashcards and study sessions
    also deleted.
    
  - Automatically created/ recorded study sessions on the completetion of a
    study session.
 
- Pivoted display of Number of Sessions Per Month Per Stack in Report feature.
  
  - ![image](https://github.com/user-attachments/assets/18bdedc6-a9d1-4e0c-849f-fe6396d35629)

## Challenges

- First time using Entity Framework. This involved watching some extra videos,
  but ultimately EF is straightforward and satisfying to use.
  
- 2nd time making a .NET MAUI app - I had some knowledge / muscle memory of how
   to use xaml, but still lots of search was required to figure out why it
  wasn't always connecting in the way that I wanted.
  
- 1st time trying to organize with MVVM.
  
  - This one was a big one. I spent lots of time pausing James Montemagno videos
    and trying to apply his style of organizing code to my code. Some things
    stuck, others I had to re-google and re-search
    to understand to get my classes to work and connect to each other.
    
- MVVM != MVC.
  
  - This seems redundant, but is something I'm still a bit fuzzy on. ViewModel
    is not a Controller, although they both introduce abstraction. I did end up
     using Dependency Injection to make my
    DbRepository service available throughout the app.
 
## Lessons Learned

- In a way, using .NET MAUI was a useful departure from console in terms of
  debugging: it no longer
  made sense to add line after line of Console.WriteLine() to debug, and
  instead I really had to use
   the stepping features, local values, and other tools in the Visual
  Studio debugger.
  
- .NET MAUI also kind of forced to use better OOP design: it made absolutely
  no sense to put UI xaml
   code together with the ViewModel, and even less sense to mix
  the UI code with the repository service.

## Areas To Improve

- Get it working cleanly before moving on. There were multiple times that I
  just got it working to a
  good-enough level, but maybe didn't have the cleanest code, or didn't
  implement INotifyPropertyChanged
   for clean 2-way data binding between vm and ui. I kept thinking that I'd
  come back to it in the end,
  but never did. In the end, I ran out of time, and it started to seem
  like a mess to wade through all
  of my code looking for times I didn't meet conventions.

- Create a new git branch for each issue or feature. This would just
  help with organization overall, but
  also keeping the commits nice and tidy instead of all over the place
  with multiple unrelated files.

- How can I use emojis or other fun visual tools in my future README files?

## Resources Used

- [James Montemagno MVVM videos](https://www.youtube.com/watch?v=sAn4RVsroF4)
  
    -[and another good video](https://www.youtube.com/watch?v=AXpTeiWtbC8)

- [dotnet series Intro to Entity Framework](https://www.youtube.com/watch?v=SryQxUeChMc)

- [MS async/await docs for best practices](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

- [How to Connect SQL Server to Visual Studio](https://www.youtube.com/watch?v=M5DhHYQlnq8)

- The C# Academy's [project guide page for guidelines](https://www.thecsharpacademy.com/project/14/flashcards)

- ChatGPT: your personal and patient debugging assistant
