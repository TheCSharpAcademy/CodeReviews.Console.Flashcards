using Flashcards.nikosnick13.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.nikosnick13.DTOs;

public class DetailStackDTO
{
    public  int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<Flashcard>? Flashcard { get; set; }

    public ICollection<StudySession>? StudySession { get; set; }
}
