using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsAssist.DreamFXX.Models;
public class Flashcard
{
    public int Id { get; set; }
    [ForeignKey("StackId")]
    public int StackId { get; set; }

    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
}
