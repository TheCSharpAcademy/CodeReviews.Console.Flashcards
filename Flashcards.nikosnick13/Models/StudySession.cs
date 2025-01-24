using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.nikosnick13.Models;

public class StudySession
{
    public int Id { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public int Score { get; set; }

    public Stack Stack { get; set; } = new Stack();
}
