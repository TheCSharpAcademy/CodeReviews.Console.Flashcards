namespace Flashcards;

class ConstraintsValidation
{
    public static string? MainMenuConstraints(string selection, Stacks? selectedStack)
    {
        string? errorMessage = null;
    
        switch(selection)
        {
            case("2"):
                if(selectedStack == null)
                {
                    errorMessage += "You have not selected a stack. ";
                }
                break;
            case("3"):
                if(selectedStack == null)
                {
                    errorMessage += "You have not selected a stack. ";
                }
                else if(!DBController.StackHasCards(selectedStack))
                {
                    errorMessage += "The stack you have selected is empty. ";
                }
                break;
            case("4"):
                if(!DBController.TableIsNotEmpty(DBController.studysessionsTableName))
                {
                    errorMessage += "You don't have any study sessions records. ";
                }
                break;
        }
        return errorMessage;
    }

    public static string? StacksMenuConstraints(string selection)
    {
        string? errorMessage = null;

        switch(selection)
        {
            case("2"):
            case("3"):
            case("4"):
                if(!DBController.TableIsNotEmpty(DBController.stacksTableName))
                {
                    errorMessage += "You don't have any stack recorded. ";
                }
                break;                                     
        }
        return errorMessage;
    }

    public static string? FlashCardsMenuConstraints(string selection, Stacks? selectedStack)
    {
        string? errorMessage = null;

        switch(selection)
        {
            case("1"):
            case("3"):
            case("4"):
                if(selectedStack!= null && !DBController.StackHasCards(selectedStack))
                {
                    errorMessage += "The selected stacks doesn't have any cards recorded. ";
                }
                break;
        }
        return errorMessage;
    }
}