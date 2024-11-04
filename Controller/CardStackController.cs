using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flashcards.TwilightSaw.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Flashcards.TwilightSaw.Controller
{
    internal class CardStackController(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public void Create(string name)
        {
            _context.Add(new CardStack(name));
            _context.SaveChanges();
        }

        public List<CardStack> Read()
        {
            return _context.CardStacks.ToList();
        }

        
    }
}
