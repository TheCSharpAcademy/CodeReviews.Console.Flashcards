namespace FlashStudy
{
    class Program
    {
        static void Main(string[] args)
        {
          SqlAccess.CheckIfDbExists();
          Input.ShowMenu();
        }
    }
}
