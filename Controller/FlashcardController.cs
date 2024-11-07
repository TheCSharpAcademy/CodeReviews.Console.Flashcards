﻿using Flashcards.TwilightSaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Flashcards.TwilightSaw.Controller
{
    internal class FlashcardController(AppDbContext context)
    {
        private AppDbContext _context = context;

        public void Create(string front, string back, int cardStackId)
        {
            context.Add(new Flashcard(front, back, cardStackId));
            context.SaveChanges();
        }

        public List<Flashcard> Read(int cardStackId)
        {
           return _context.Flashcards.Where(t => t.CardStackId == cardStackId).ToList();
        }

        public void Delete(int flashcardId, int chosenStackCardStackId)
        {
            _context.Flashcards.Where(flashcard => flashcard.CardStackId == chosenStackCardStackId && flashcard.Id == flashcardId).ExecuteDelete();
        }

       
    }
}
