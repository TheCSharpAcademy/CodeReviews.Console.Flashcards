using System;
using FlashStudy.Models;

namespace FlashStudy.DTOs
{
  public class SessionToViewDTO
  {
    public string CreatedOn { get; set; }
    public int Score { get; set; }

    public SessionToViewDTO(Session session)
    {
      CreatedOn = session.CreatedOn;
      Score = session.Score;
    }
  }
}
