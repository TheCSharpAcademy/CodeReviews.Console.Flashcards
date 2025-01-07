namespace FlashcardStack.AshtonLeeSeloka.MenuEnums;

internal class MenuEnums
{
	public enum MainMenu 
	{
		View_Reports,
		Manage_Stacks,
		Study,
		Exit
	}

	public enum ManageStacks 
	{
		Manage_Existing_Stacks,
		Create_New_Stack,
		Exit
	}

	public enum ManageExistingStack
	{
		Create_New_Card,
		Edit_Cards,
		Delete_Cards,
		Delete_Stack,
		Exit

	}

	public enum ManageCards
	{
		Create_New_Card,
		Edit_Cards,
		Delete_Cards,
		Exit
	}

	public enum StudyOptions
	{
		View_All_Cards,
		Play,
		Exit
	}
}
