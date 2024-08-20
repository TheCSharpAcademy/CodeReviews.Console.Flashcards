using Flashcards.Domain.Entities;
using Flashcards.Data.Context;
using Microsoft.EntityFrameworkCore;

using Flashcards.Domain.Interfaces;

namespace Flashcards.Data.Repositories
{
    public class StackRepository : IStackRepository
    {
        private readonly ApplicationDbContext _context;

        public StackRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Stack stack)
        {
            if (_context.Stacks.Any(s => s.Name == stack.Name))
            {
                throw new InvalidOperationException("Stack name must be unique.");
            }

            _context.Stacks.Add(stack);
            _context.SaveChanges();
        }


        public void Update(Stack stack)
        {
            _context.Stacks.Update(stack);
            _context.SaveChanges();
        }

        public void Delete(int stackId)
        {
            var stack = _context.Stacks.Include(s => s.Flashcards)
                                       .Include(s => s.StudySessions)
                                       .FirstOrDefault(s => s.Id == stackId);
            if (stack != null)
            {
                _context.Flashcards.RemoveRange(stack.Flashcards);
                _context.StudySessions.RemoveRange(stack.StudySessions);
                _context.Stacks.Remove(stack);
                _context.SaveChanges();
            }
        }


        public Stack GetById(int id)
        {
            var stack = _context.Stacks.Include(s => s.Flashcards)
                                       .Include(s => s.StudySessions)
                                       .FirstOrDefault(s => s.Id == id);
            if (stack == null)
            {
                throw new KeyNotFoundException($"Stack with ID {id} was not found.");
            }
            return stack;
        }

        public IEnumerable<Stack> GetAll()
        {
            return _context.Stacks.Include(s => s.Flashcards)
                                  .Include(s => s.StudySessions)
                                  .ToList();
        }

        public Stack? GetByName(string name)
        {
            return _context.Stacks.FirstOrDefault(s => s.Name == name);
        }


    }
}
