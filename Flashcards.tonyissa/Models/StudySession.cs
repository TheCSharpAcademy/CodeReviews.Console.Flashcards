﻿namespace Flashcards.Models;

public class StudySession
{
    public int Id { get; set; }
    public int StackId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    public StudySession()
    {

    }
}