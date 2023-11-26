using Flashcards.wkktoria.Models.Dtos;

namespace Flashcards.wkktoria.Services.Helpers;

internal static class ReportDataHelper
{
    internal static List<ReportDataDto> Load(StackService stackService, SessionService sessionService)
    {
        var stacks = stackService.GetAll();
        var reportData = new List<ReportDataDto>();

        foreach (var stack in stacks)
        {
            var fullStack = stackService.GetByDtoId(stack.DtoId);
            var sessions = sessionService.GetAll(fullStack!.Id);

            reportData.AddRange(sessions.Select(session => new ReportDataDto
            {
                StackId = fullStack.Id,
                StackName = fullStack.Name,
                SessionYear = session.FinishedDate.Year,
                SessionMonth = session.FinishedDate.Month,
                Score = session.Score
            }));
        }

        return reportData;
    }
}