namespace FlashCards.DTOs
{
    internal class DeckDTO
    {
        internal int Id { get; set; }
        internal string Name { get; set; }


        internal DeckDTO()
        {
            Name = string.Empty;
        }

        internal DeckDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
