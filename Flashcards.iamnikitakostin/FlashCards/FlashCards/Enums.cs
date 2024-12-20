namespace FlashCards
{
    internal class Enums
    {
        internal enum MainMenuOptions
        {
            Study,
            ManageStacks,
            ManageFlashCards,
            ViewStudyData,
            About,
            Quit
        }

        internal enum ManageNavMenuOptions
        {
            Add,
            ManageAll,
            Back
        }

        internal enum ManageStackMenuOptions
        {
            ViewFullInfo,
            Edit,
            Delete,
            Back
        }

        internal enum StandardMenuOptions
        {
            Back
        }

        internal enum ConfirmationMenuOptions
        {
            Yes,
            No
        }

        internal enum ManageStackEditOptions
        {
            Name
        }

        internal enum ManageFlashcardMenuOptions
        {
            ViewFullInfo,
            Edit,
            Delete,
            Back
        }

        internal enum ManageFlashcardEditOptions
        {
            Front,
            Back
        }

        internal enum StudyMenuOptions
        {
            NextCard,
            Back
        }

        internal enum BreakPeriodInSecondsOptions 
        {
            Minute = 60,
            ThirtyMinutes = 1800,
            Hour = 3600,
            SixHours = 21600,
            TwelveHours = 43200,
            Day = 86400,
            TwoDays = 172800,
            FiveDays = 432000,
            Week = 604800,
            TwoWeeks = 1209600,
            Month = 2629746,
        }
    }
}
