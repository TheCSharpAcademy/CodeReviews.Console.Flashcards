using Flashcards.yemiOdetola;

string connectionString = "Server=localhost,1433;Database=FlashCards;User Id=sa;Password=<YourStrong@Passw0rd>;TrustServerCertificate=True;";
var dbQuery = new DatabaseQueries(connectionString);

dbQuery.ConnectDatabase();
dbQuery.CreateTables();
MenuOptions.Start();
