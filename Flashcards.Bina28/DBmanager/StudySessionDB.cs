


using Flashcards.Bina28.Models;
using Microsoft.Data.SqlClient;



namespace Flashcards.Bina28.DBmanager;

internal class StudySessionDB
{
	private string connectionString;
	public StudySessionDB()
	{

		connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
	}

	internal bool CreateSession(object date, string stackName, int wrongAnswer, int rightAnswer, int totalQuestions)
	{

		string query = "INSERT INTO StudySessionLog (SessionDate,  StackName, NumberOfWrongAnswers , NumberOfRightAnswers, TotalQuestions) VALUES (@date, @stackName, @wrongAnswer, @rightAnswer,  @totalQuestions)";
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			SqlCommand command = new SqlCommand(query, connection);
			command.Parameters.AddWithValue("@date", date);
			command.Parameters.AddWithValue("@stackName", stackName);
			command.Parameters.AddWithValue("@wrongAnswer", wrongAnswer);
			command.Parameters.AddWithValue("@rightAnswer", rightAnswer);
			command.Parameters.AddWithValue("@totalQuestions", totalQuestions);
			try
			{
				connection.Open();
				int result = command.ExecuteNonQuery();
				return result > 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
				return false;
			}
		}
	}

	internal List<StudySessionDto> GetAllRecords()
	{
		var sessions = new List<StudySessionDto>();

		try
		{
			using (var connection = new SqlConnection(connectionString))
			{
				connection.Open();

				string sql = @"
                        SELECT SessionDate, StackName, NumberOfWrongAnswers, NumberOfRightAnswers, TotalQuestions
                        FROM StudySessionLog";

				using (var command = new SqlCommand(sql, connection))
				{
					using (var reader = command.ExecuteReader())
					{
						while (reader.Read())
						{
							var session = new StudySessionDto(
								Convert.ToDateTime(reader["SessionDate"]), // DateTime
								reader["StackName"].ToString(), // string
								Convert.ToInt32(reader["NumberOfWrongAnswers"]), // int
								Convert.ToInt32(reader["NumberOfRightAnswers"]), // int
								Convert.ToInt32(reader["TotalQuestions"]) // int
							);

							sessions.Add(session);
						}
					}
				}
			}
		}
		catch (SqlException ex)
		{
			Console.WriteLine("SQL Error: " + ex.Message);


		}
		catch (Exception ex)
		{
			Console.WriteLine("Error: " + ex.Message);


		}

		return sessions; // Return a list of StudySessionDto
	}

	public void CreateStudySessionLogTable()
	{
		string createTableQuery = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudySessionLog')
        BEGIN
            CREATE TABLE StudySessionLog (
                SessionID INT IDENTITY(1,1) PRIMARY KEY,  -- Auto-incremented unique identifier
                SessionDate DATE NOT NULL,                 -- Date of the study session
                StackName NVARCHAR(100) NOT NULL,          -- Name of the flashcard stack
                NumberOfWrongAnswers INT NOT NULL,         -- Number of incorrect answers
                NumberOfRightAnswers INT NOT NULL,         -- Number of correct answers
                TotalQuestions INT NOT NULL,               -- Total number of questions
                CONSTRAINT CK_StudySessionLog_Answers CHECK (NumberOfRightAnswers + NumberOfWrongAnswers <= TotalQuestions)  -- Ensure total answers do not exceed total questions
            );
            PRINT 'StudySessionLog table created successfully.';
        END";

		// Execute the query to create the table
		using (SqlConnection connection = new SqlConnection(connectionString))
		{
			try
			{
				connection.Open();

				using (SqlCommand command = new SqlCommand(createTableQuery, connection))
				{
					command.ExecuteNonQuery();

				}

			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}
		}
	}

}



