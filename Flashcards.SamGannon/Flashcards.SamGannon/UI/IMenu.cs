using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.UI
{
    public interface IMenu
    {
        IDataAccess DataAccess { get; }
        void ShowMenu();
    }
}
