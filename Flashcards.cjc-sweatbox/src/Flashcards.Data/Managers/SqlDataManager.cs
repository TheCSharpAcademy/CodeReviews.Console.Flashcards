namespace Flashcards.Data.Managers;

/// <summary>
/// Partial class for non entity specific data manager methods against an T-SQL database.
/// </summary>
public partial class SqlDataManager
{
    #region Properties

    public string ConnectionString { get; init; }

    public string Schema => "dbo";

    #endregion
    #region Constructors

    public SqlDataManager(string connectionString)
    {
        ConnectionString = connectionString;
    }

    #endregion
    #region Methods

    // TODO.

    #endregion
}
