using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Flashcards.AnaClos.Models;

public class FlashCard
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
}
