namespace FlashCards
{
    internal class FlashCard
    {
        public int StackID { get; set; }
        public string StackName { get; set; } = string.Empty;
        public int CardID { get; set; }
        public string FrontText { get; set; } = string.Empty;
        public string BackText { get; set; } = string.Empty;
    }
}