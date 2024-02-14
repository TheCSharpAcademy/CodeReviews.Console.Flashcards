using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Data;


DbAccess context = new();
context.CreateDatabase();
StateManager stateManager = new(context);
stateManager.Run(new MainMenuState(stateManager));