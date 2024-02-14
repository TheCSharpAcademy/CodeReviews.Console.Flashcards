using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Data;

DbAccess.CreateDatabase();
StateManager stateManager = new();
stateManager.Run(new MainMenuState(stateManager));