﻿namespace Flashcards.Models;

public class Flashcard
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Answer { get; set; }
    public int CategoryId { get; set; }
}