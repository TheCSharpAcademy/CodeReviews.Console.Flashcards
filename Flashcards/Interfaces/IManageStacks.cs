public interface IManageStacks
{
    public void CreateStack(string name);
    public void DeleteStack(string name);
    public List<Stack> GetStacks();
}