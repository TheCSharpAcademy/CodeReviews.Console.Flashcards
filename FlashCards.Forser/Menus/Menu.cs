namespace FlashCards.Forser
{
    public class Menu
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public override string ToString() => Text;

        internal static Style HighLightStyle => new(
            Color.LightGreen,
            Color.Black,
            Decoration.None
        );
        internal static void RenderTitle(string title)
        {
            var rule = new Rule($"[green]{title}[/]");
            AnsiConsole.Write(rule);
        }
    }
}