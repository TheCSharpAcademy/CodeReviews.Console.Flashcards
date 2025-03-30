namespace FlashCards
{
    internal class FlashCardDto
    {
        public int CardID { get; set; }
        public string FrontText { get; set; } = string.Empty;
        public string BackText { get; set; } = string.Empty;
    }
}
