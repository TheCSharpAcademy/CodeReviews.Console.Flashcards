public class StackController : IManageStacks
{
    private IManageStacks _stackRepo;

    public StackController(IManageStacks stackRepo)
    {
        _stackRepo = stackRepo;
    }

    public void CreateStack(string name)
    {
        throw new NotImplementedException();
    }

    public void DeleteStack(string name)
    {
        throw new NotImplementedException();
    }

    public List<Stack> GetStacks() => _stackRepo.GetStacks();
}