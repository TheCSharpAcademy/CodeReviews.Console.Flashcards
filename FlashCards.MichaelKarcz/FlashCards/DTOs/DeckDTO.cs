namespace FlashCards.DTOs
{
    internal class DeckDto
    {
        internal int Id { get; set; }
        internal string Name { get; set; }


        internal DeckDto()
        {
            Name = string.Empty;
        }

        internal DeckDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
