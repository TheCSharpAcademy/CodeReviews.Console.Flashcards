using Flashcards.Models;

namespace Flashcard_Application.DataServices;

public class DatabaseSeeding
{
    public CardStack cardStack1 = new CardStack("Data Types", "A data type specifies the size and type of variable values.");
    public CardStack cardStack2 = new CardStack("Access Modifiers", "Controls the level of access to a given class or member of a class");

    public Flashcard intCard = new Flashcard("int", "Whole numbers.", 1);
    public Flashcard stringCard = new Flashcard("string", "Sequence of characters.", 1);
    public Flashcard boolCard = new Flashcard("bool", "Boolean (true / false).", 1);
    public Flashcard charCard = new Flashcard("char", "Single character.", 1);

    public Flashcard publicCard = new Flashcard("public", "The code is accessible for all classes.", 2);
    public Flashcard privateCard = new Flashcard("private", "The code is only accessible within the same class.", 2);
    public Flashcard protectedCard = new Flashcard("protected", "The code is accessible within the same class, or in a class that is inherited from that class.", 2);
    public Flashcard internalCard = new Flashcard("internal", "The code is only accessible within its own assembly, but not from another assembly.", 2);
}
