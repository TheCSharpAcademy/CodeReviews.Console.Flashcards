namespace Models;

public class CreateStackDto
{
    public CreateStackDto(string name)
    {
        Name = name;
    }

    public string Name { get; set; }


    public required IEnumerable<CreateFlashcardDto> Flashcards { get; set; }
}

