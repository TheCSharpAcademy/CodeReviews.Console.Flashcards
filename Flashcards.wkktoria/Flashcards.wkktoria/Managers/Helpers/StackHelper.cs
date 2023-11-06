using Flashcards.wkktoria.Models;
using Flashcards.wkktoria.Services;
using Flashcards.wkktoria.UserInteractions;

namespace Flashcards.wkktoria.Managers.Helpers;

internal static class StackHelper
{
    internal static Stack Choose(StackService stackService)
    {
        Stack? stack;

        var stacks = stackService.GetAll();

        TableVisualisation.ShowStacksTable(stacks);

        do
        {
            var id = UserInput.GetNumberInput("Enter id of stack to study.");
            stack = stackService.GetByDtoId(id);

            if (stack == null) UserOutput.ErrorMessage("No stack found.");
        } while (stack == null);

        return stack;
    }
}