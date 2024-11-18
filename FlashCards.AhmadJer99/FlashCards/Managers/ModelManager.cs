
namespace FlashCards.Managers;

public abstract class ModelManager<T>
{

    protected abstract void DeleteModel(int stackId);
    protected abstract void UpdateModel(int stackId, T modifiedModel);
    protected abstract void AddNewModel();

}
