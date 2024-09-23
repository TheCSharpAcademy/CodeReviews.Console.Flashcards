using FlashcardsLibrary.Models;

namespace FlashcardsLibrary;

public class DatabaseQueries : RunCommand<DatabaseQueries>
{
    private void CreateTableFlashcards()
    {
        DatabaseConnection.Run.Execute("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Flashcards' and xtype='U') " +
            "CREATE TABLE [Flashcards]" +
            "(FlashcardId   INT         IDENTITY(1,1)," +
            "Front          VARCHAR(50) NOT NULL," +
            "Back           VARCHAR(50) NOT NULL," +
            "StackId        INT         NOT NULL," +
            "PRIMARY KEY(FlashcardId)," +
            "FOREIGN KEY(StackId) REFERENCES Stacks(StackId)" +
            "ON UPDATE CASCADE ON DELETE CASCADE)");
    }

    private void CreateTableStacks()
    {
        DatabaseConnection.Run.Execute("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stacks' and xtype='U') " +
            "CREATE TABLE [Stacks]" +
            $"(StackId      INT         IDENTITY(1,1)," +
            $"StackName     VARCHAR(50) NOT NULL UNIQUE," +
            $"PRIMARY KEY(StackId))");
    }

    private void CreateTableStudySessions()
    {
        DatabaseConnection.Run.Execute("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Study sessions' and xtype='U') " +
            "CREATE TABLE [Study sessions]" +
            "(StudySessionId   INT         IDENTITY(1,1)," +
            "Date              DATETIME    NOT NULL," +         
            "Score             INT         NOT NULL," +
            "StackName         VARCHAR(50) NOT NULL," +
            "PRIMARY KEY(StudySessionId)," +
            "FOREIGN KEY(StackName) REFERENCES Stacks(StackName)" +
            "ON UPDATE CASCADE ON DELETE CASCADE)");
    }

    public void InsertStudySession(DateTime date, int score, string stackName)
    {
        object[] parameters = { new { @Date = date, @Score = score, @StackName = stackName } };
        DatabaseConnection.Run.Execute("INSERT INTO [Study sessions] (Date, Score, StackName) VALUES (@Date, @Score, @StackName)", parameters);
    }

    public void InsertStacks(string stackName)
    {
        object[] parameters = { new { @StackName = stackName } };
        DatabaseConnection.Run.Execute("INSERT INTO [Stacks] (StackName) VALUES (@StackName)", parameters);   
    }

    public void DeleteStacks(string stackName)
    {
        object[] parameters = { new { @StackName = stackName } };
        DatabaseConnection.Run.Execute("DELETE FROM [Stacks] WHERE StackName = @StackName", parameters);
    }

    public void UpdateStacks(string oldStackName, string newStackName)
    {
        object[] parameters = { new { @NewStackName = newStackName, @OldStackName = oldStackName } };
        DatabaseConnection.Run.Execute("UPDATE [Stacks] SET StackName = @NewStackName WHERE StackName = @OldStackName", parameters);           
    }

    public void InsertFlashcards(int stackId, string front, string back)
    {
        object[] parameters = { new {@Front = front, @Back = back, @StackId = stackId} };
        DatabaseConnection.Run.Execute("INSERT INTO [Flashcards] (Front, Back, StackId) VALUES (@Front, @Back, @StackId)", parameters);
    }

    public void DeleteFlashcards(int flashcardId)
    {
        object[] parameters = { new { @FlashcardId = flashcardId } };
        DatabaseConnection.Run.Execute("DELETE FROM [Flashcards] WHERE FlashcardId = @FlashcardId", parameters);
    }

    public void UpdateFlashcards(int flashcardId, string front, string back)
    {
        object[] parameters = { new { @Front = front, @Back = back, @FlashcardId = flashcardId } };
        DatabaseConnection.Run.Execute("UPDATE [Flashcards] SET Front = @Front, Back = @Back WHERE FlashcardId = @FlashcardId", parameters);
    }

    public List<Stacks> SelectAllStacks()
    {
        return DatabaseConnection.Run.Query<Stacks>("SELECT * FROM [Stacks]");
    }

    public List<Flashcards> SelectAllFlashcards()
    {
        return DatabaseConnection.Run.Query<Flashcards>("SELECT * FROM [Flashcards]");
    }

    public List<StudySessions> SelectAllStudySessions()
    {
        return DatabaseConnection.Run.Query<StudySessions>("SELECT * FROM [Study sessions]");
    }

    public List<PivotReports> SelectPivotSessionsPerMonth(int year, string type)
    {
        return DatabaseConnection.Run.Query<PivotReports>("SELECT [StackName], [1] as \"January\", [2] as \"February\", [3] as \"March\", [4] as \"April\", [5] as \"May\", [6] as \"June\"," +
            " [7] as \"July\", [8] as \"August\", [9] as \"September\", [10] as \"October\", [11] as \"November\", [12] as \"December\"" +
            $"FROM (SELECT [StackName], Month(Date) as TMonth, [Score] FROM [Study sessions] WHERE Year(Date) = '{year}')" +
            "source PIVOT " +
            $"({type}(Score) FOR TMonth IN ( [1], [2], [3], [4], [5], [6], [7], [8], [9], [10], [11], [12] ))" +
            "AS pivotSessionsPerMonth");
    }

    public void CreateTablesIfNotExist()
    {
        Run.CreateTableFlashcards();
        Run.CreateTableStacks();
        Run.CreateTableStudySessions();
    }
}

