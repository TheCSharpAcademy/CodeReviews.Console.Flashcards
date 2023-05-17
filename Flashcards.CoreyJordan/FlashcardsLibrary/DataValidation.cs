namespace FlashcardsLibrary;
public static class DataValidation
{
    public static bool IsUniqueDeckName(string name)
    {
        bool isUniqueDeckName = false;

		try
		{
			isUniqueDeckName = CrudController.DeckExists(name);
		}
		catch (Exception)
		{
			throw;
		}

        return isUniqueDeckName;
    }
}
