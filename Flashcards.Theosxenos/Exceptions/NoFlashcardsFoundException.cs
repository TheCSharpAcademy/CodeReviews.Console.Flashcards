namespace Flashcards.Exceptions;

public class NoFlashcardsFoundException() : NotFoundException("No flashcards found. Please create one first.");