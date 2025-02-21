# Flashcards Application Requirements

## Overview
This application allows users to create and study Stacks of Flashcards.

## Database Structure
1. Two main tables: Stacks and Flashcards
    - These tables should be linked by a foreign key

2. Stack table requirements:
    - Each stack must have a unique name

3. Flashcard table requirements:
    - Every flashcard must be associated with a stack
    - If a stack is deleted, all its flashcards should be deleted (cascading delete)

4. Study Session table:
    - Linked to the Stack table
    - If a stack is deleted, its study sessions should be deleted (cascading delete)
    - This table should only allow insert operations (no updates or deletes)

## Functionality Requirements

### Flashcard Management
1. Users can create, read, update, and delete flashcards within stacks
2. When displaying flashcards to users:
    - Use DTOs to hide the stack ID
    - Flashcard IDs should always start from 1 and have no gaps
    - Example: If there are 10 cards and number 5 is deleted, the table should show IDs from 1 to 9

### Study Sessions
1. Implement a "Study Session" area where users can study the stacks
2. Each study session should be stored with:
    - Date
    - Score
    - Associated stack

### User Interface
1. Provide functionality for users to view all their study sessions
2. Ensure the UI reflects the logical IDs for flashcards (1 to N, without gaps)

## Technical Requirements
1. Use DTOs (Data Transfer Objects) for displaying flashcard information to users
2. Implement proper foreign key relationships in the database
3. Ensure cascading deletes are set up correctly in the database schema

Created by @DreamFXX - 2025