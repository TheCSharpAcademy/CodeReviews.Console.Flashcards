using Flashcards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Repository
{
    public interface IStackRepository
    {
        void AddStack(string stackName);
        List<Stack> GetAllStacks();

        Stack? GetStackById(int stackId);
        void DeleteStack(int stackId);
    }
}
