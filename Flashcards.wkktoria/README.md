# Flashcards

## Requirements

- [X] User should be able to create stacks of flashcards.
- [X] There should be two tables in database, for stacks and flashcards. The tables should be linked be foreign key.
- [X] Stacks should have unique names.
- [X] Every flashcards should be part of stack. If stack is deleted, the same should happen with linked flashcards.
- [X] DTO should be used to show flashcards without IDs of stacks they belong to.
- [X] When showing, flashcards IDs should always start with 1, without gaps between numbers.
- [X] There should be study session area, where it's possible to study stacks. All study sessions should be stored with date and score.
- [X] Study session and stack should be linked. If stack is deleted, linked study session should also be deleted.
- [X] There should be possibility to see all study sessions. Study session cannot be updated and deleted.

## Challenge

Create report systems. One to report number of sessions per month per stack, and another one to report average score per month per stack.