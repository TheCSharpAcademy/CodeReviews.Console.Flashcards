using Flashcards.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

AppDbContext db = new AppDbContext();

//Stack questions = new Stack {
//    Name = "Questions"
//};

//ICollection<Flashcard> flashcards = new List<Flashcard>() {
//    new Flashcard {
//        Stack = questions,
//        Question = "Test",
//        Answer = "Test",
//    },
//    new Flashcard {
//        Stack = questions,
//        Question = "Test 2",
//        Answer = "Test 2"
//    }
//};

//db.Stacks.Add(questions);
//db.Flashcards.AddRange(flashcards);
//db.SaveChanges();

Stack questions = db.Stacks.FirstOrDefault(s => s.Name == "Questions");

db.Remove<Stack>(questions);
db.SaveChanges();


