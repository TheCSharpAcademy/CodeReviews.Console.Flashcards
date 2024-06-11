public class StackController : IManageStacks
{
    private IManageStacks _stackRepo;

    public StackController(IManageStacks stackRepo)
    {
        _stackRepo = stackRepo;
    }

    public void CreateStack(string name)
    {
        if (_stackRepo.GetStacks().FirstOrDefault(x => x.Name.ToLower() == name) != null)
        {
            Console.WriteLine("That stack name already exists. Cannot insert.");
        }
        else
        {
            _stackRepo.CreateStack(name);
        }
    }

    public void DeleteStack(string name)
    {
        if (_stackRepo.GetStacks().FirstOrDefault(x => x.Name.ToLower() == name.ToLower()) == null)
        {
            Console.WriteLine("That stack does not exist, cannot delete.");
        }
        else
        {
            _stackRepo.DeleteStack(name);
        }
    }

    public List<Stack> GetStacks() => _stackRepo.GetStacks();
}
