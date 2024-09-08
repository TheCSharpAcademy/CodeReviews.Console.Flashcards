using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flash_Cards.Lawang.Controller;

public class StudyController
{
    private string _connectionString;
    public StudyController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void CreateStudyTable()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL =
                @"IF NOT EXISTS
                (
                    SELECT * FROM sys.tables
                    WHERE name = 'Study_Sessions' AND schema_id = SCHEMA_ID('dbo')
                )
                BEGIN
                    CREATE TABLE Study_Sessions
                    (
                        Id INT PRIMARY KEY IDENTITY(1, 1),
                        Date DATETIME2,
                        Score INT,
                        StackId INT,
                        FOREIGN KEY(StackId) REFERENCES stacks(Id) ON DELETE CASCADE 

                    );
                END";

            connection.Execute(createSQL);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public int CreateStudySession(int score, int stackId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string createSQL =
                @"INSERT INTO Study_Sessions
                (Date, Score, StackId)
                VALUES(@date, @score, @stackId)";

            var param = new { @date = DateTime.Now.Date, @score = score, @stackId = stackId };

            return connection.Execute(createSQL, param);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
        return 0;
    }

    public void ViewAllStudySession()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string readSQL =
                @"SELECT * FROM Study_Sessions";

            var reader = connection.ExecuteReader(readSQL);
            var table = new Table()
            .Border(TableBorder.Rounded)
            .Expand()
            .BorderColor(Color.Aqua)
            .ShowRowSeparators()
            .Title("STUDY-SESSIONS", Color.CadetBlue);



            table.AddColumns(new TableColumn[]
            {
            new TableColumn("[darkgreen bold]Id[/]").Centered(),
            new TableColumn("[darkcyan bold]Date[/]").Centered(),
            new TableColumn("[darkcyan bold]Score[/]").Centered(),
            });

            while (reader.Read())
            {
                table.AddRow(reader.GetInt32(0).ToString(),
                 reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                 reader.GetInt32(2).ToString());
            }

            AnsiConsole.Write(table);
        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void ViewNumberOfSessionsPerMonthPerStack(int year)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string readSQL =
                $@"SELECT
                    s.Name AS StackName,
                    ISNULL([January], 0) AS [January],
                    ISNULL([February], 0) AS [February],
                    ISNULL([March], 0) AS [March],
                    ISNULL([April], 0) AS [April],
                    ISNULL([May], 0) AS [May],
                    ISNULL([June], 0) AS [June],
                    ISNULL([July], 0) AS [July],
                    ISNULL([August], 0) AS [August],
                    ISNULL([September], 0) AS [September],
                    ISNULL([October], 0) AS [October],
                    ISNULL([November], 0) AS [November],
                    ISNULL([December], 0) AS [December]
                FROM 
                    (
                        SELECT DATENAME(MONTH, Date) AS Month, COUNT(Id) AS TotalSessions, StackId as StackId
                        FROM Study_Sessions
                        WHERE YEAR(Date) = {year}
                        GROUP BY DATENAME(MONTH, Date), StackId
                    ) AS Source
                PIVOT(SUM(TotalSessions) FOR Month IN ([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) AS data
                JOIN dbo.stacks s
                ON s.Id = data.StackId";

            var table = new Table()
                .Border(TableBorder.Ascii)
                .Expand()
                .BorderColor(Color.Aqua)
                .ShowRowSeparators()
                .Title($"Total Session per month for: {year}", Color.CadetBlue);

            table.AddColumns(new TableColumn[]
            {
                    new TableColumn("[darkgreen bold]Stack Name[/]").Centered(),
                    new TableColumn("[darkcyan bold]January[/]").Centered(),
                    new TableColumn("[darkcyan bold]February[/]").Centered(),
                    new TableColumn("[darkcyan bold]March[/]").Centered(),
                    new TableColumn("[darkcyan bold]April[/]").Centered(),
                    new TableColumn("[darkcyan bold]May[/]").Centered(),
                    new TableColumn("[darkcyan bold]June[/]").Centered(),
                    new TableColumn("[darkcyan bold]July[/]").Centered(),
                    new TableColumn("[darkcyan bold]August[/]").Centered(),
                    new TableColumn("[darkcyan bold]September[/]").Centered(),
                    new TableColumn("[darkcyan bold]October[/]").Centered(),
                    new TableColumn("[darkcyan bold]November[/]").Centered(),
                    new TableColumn("[darkcyan bold]December[/]").Centered()
            });

            var reader = connection.ExecuteReader(readSQL);
            if (!reader.Read())
            {
                Panel nullPanel = new Panel(new Markup("[red bold]STUDY-SESSION TABLE IS EMPTY!!![/]"))
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.IndianRed1_1)
                .Padding(1, 1, 1, 1)
                .Header("Result");


                AnsiConsole.Write(nullPanel);
                return;
            }
            else
            {
                while (reader.Read())
                {
                    table.AddRow(
                        reader.GetString(0),
                        reader.GetInt32(1).ToString(),
                        reader.GetInt32(2).ToString(),
                        reader.GetInt32(3).ToString(),
                        reader.GetInt32(4).ToString(),
                        reader.GetInt32(5).ToString(),
                        reader.GetInt32(6).ToString(),
                        reader.GetInt32(7).ToString(),
                        reader.GetInt32(8).ToString(),
                        reader.GetInt32(9).ToString(),
                        reader.GetInt32(10).ToString(),
                        reader.GetInt32(11).ToString(),
                        reader.GetInt32(12).ToString()
                    );
                }

                AnsiConsole.Write(table);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void ViewAverageOfSessionPerMonthPerStack(int year)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            string readSQL =
                $@"SELECT
                    stacks.Name AS Stack_Name,
                    ISNULL([January], 0) AS [January],
                    ISNULL([February], 0) AS [February],
                    ISNULL([March], 0) AS [March],
                    ISNULL([April], 0) AS [April],
                    ISNULL([May], 0) AS [May],
                    ISNULL([June], 0) AS [June],
                    ISNULL([July], 0) AS [July], 
                    ISNULL([August], 0) AS [August],
                    ISNULL([September], 0) AS [September],
                    ISNULL([October], 0) AS [October],
                    ISNULL([November], 0) AS [November],
                    ISNULL([December], 0) AS [December]
                FROM
                (
                    SELECT StackId AS StackId, AVG(Score) AS AverageScore, DATENAME(MONTH, Date) AS [Month]
                    FROM Study_Sessions
                    WHERE YEAR(Date) = {year}
                    GROUP BY DATENAME(MONTH, Date), StackId
                ) AS Source
                PIVOT(AVG(AverageScore) FOR Month IN([January], [February], [March], [April], [May], [June], [July], [August], [September], [October], [November], [December])) AS data
                JOIN stacks
                ON stacks.Id = StackId";

            var table = new Table()
               .Border(TableBorder.Ascii)
               .Expand()
               .BorderColor(Color.Aqua)
               .ShowRowSeparators()
               .Title($"Average per month for: {year}", Color.CadetBlue);

            table.AddColumns(new TableColumn[]
            {
                    new TableColumn("[darkgreen bold]Stack Name[/]").Centered(),
                    new TableColumn("[darkcyan bold]January[/]").Centered(),
                    new TableColumn("[darkcyan bold]February[/]").Centered(),
                    new TableColumn("[darkcyan bold]March[/]").Centered(),
                    new TableColumn("[darkcyan bold]April[/]").Centered(),
                    new TableColumn("[darkcyan bold]May[/]").Centered(),
                    new TableColumn("[darkcyan bold]June[/]").Centered(),
                    new TableColumn("[darkcyan bold]July[/]").Centered(),
                    new TableColumn("[darkcyan bold]August[/]").Centered(),
                    new TableColumn("[darkcyan bold]September[/]").Centered(),
                    new TableColumn("[darkcyan bold]October[/]").Centered(),
                    new TableColumn("[darkcyan bold]November[/]").Centered(),
                    new TableColumn("[darkcyan bold]December[/]").Centered()
            });

            var reader = connection.ExecuteReader(readSQL);
            if (!reader.Read())
            {
                Panel nullPanel = new Panel(new Markup("[red bold]STUDY-SESSION TABLE IS EMPTY!!![/]"))
                .Border(BoxBorder.Heavy)
                .BorderColor(Color.IndianRed1_1)
                .Padding(1, 1, 1, 1)
                .Header("Result");


                AnsiConsole.Write(nullPanel);
                return;
            }
            else
            {
                while (reader.Read())
                {
                    table.AddRow(
                        reader.GetString(0),
                        reader.GetInt32(1).ToString(),
                        reader.GetInt32(2).ToString(),
                        reader.GetInt32(3).ToString(),
                        reader.GetInt32(4).ToString(),
                        reader.GetInt32(5).ToString(),
                        reader.GetInt32(6).ToString(),
                        reader.GetInt32(7).ToString(),
                        reader.GetInt32(8).ToString(),
                        reader.GetInt32(9).ToString(),
                        reader.GetInt32(10).ToString(),
                        reader.GetInt32(11).ToString(),
                        reader.GetInt32(12).ToString()
                    );
                }

                AnsiConsole.Write(table);
            }


        }
        catch (SqlException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
