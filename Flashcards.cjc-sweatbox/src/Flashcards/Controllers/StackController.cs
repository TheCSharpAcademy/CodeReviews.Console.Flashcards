using Flashcards.Data.Managers;
using Flashcards.Models;

namespace Flashcards.Controllers;

/// <summary>
/// Controller for all interactions between the Stack model and entity.
/// </summary>
public class StackController
{
    #region Fields

    private readonly SqlDataManager _dataManager;

    #endregion
    #region Constructors

    public StackController(string connectionString)
    {
        _dataManager = new SqlDataManager(connectionString);
    }

    #endregion
    #region Methods

    public void AddStack(string name)
    {
        _dataManager.AddStack(name);
    }

    public void DeleteStack(int id)
    {
        _dataManager.DeleteStack(id);
    }

    public StackDto GetStack(string name)
    {
        return new StackDto(_dataManager.GetStack(name));
    }

    public IReadOnlyList<StackDto> GetStacks()
    {
        return _dataManager.GetStacks().Select(x => new StackDto(x)).ToList();
    }

    public void SetStack(int id, string name)
    {
        _dataManager.SetStack(id, name);
    }

    #endregion
}
