using System.ComponentModel.DataAnnotations;
namespace Flashcards.DreamFXX.DTOs;
public class CardStackDetailsDto
{
    [Required]
    public string CardStackName { get; set; }
    public List<CardsDTO>? Cards { get; set; }
}

