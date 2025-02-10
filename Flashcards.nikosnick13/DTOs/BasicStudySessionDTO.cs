using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.nikosnick13.DTOs;

internal class BasicStudySessionDTO
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime Date { get; set; }
    public string StackName { get; set; }

    public BasicStudySessionDTO(int id, int score, DateTime date, string stackName)
    {
        Id = id;
        Score = score;
        Date = date;
        StackName = stackName;
    }
}
