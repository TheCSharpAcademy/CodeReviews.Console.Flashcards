using System.Configuration;

string? connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

if (connectionString == null )
{
    Console.WriteLine("No connection detection.");

}

Console.WriteLine("Intializing connection...");