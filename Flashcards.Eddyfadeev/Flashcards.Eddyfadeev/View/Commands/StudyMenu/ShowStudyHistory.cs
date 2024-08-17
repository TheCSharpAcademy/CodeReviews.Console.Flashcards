﻿using Flashcards.Interfaces.Report;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.StudyMenu;

/// <summary>
/// Represents a command that displays the study history to the user and provides options to save it as a PDF.
/// </summary>
internal class ShowStudyHistory : ICommand
{
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IReportGenerator _reportGenerator;
    
    public ShowStudyHistory(
        IStudySessionsRepository studySessionsRepository,
        IReportGenerator reportGenerator
        )
    {
        _studySessionsRepository = studySessionsRepository;
        _reportGenerator = reportGenerator;
    }
    
    public void Execute()
    {
        var studySessions = _studySessionsRepository.GetAll().ToList();
        
        var table = _reportGenerator.GetReportToDisplay(studySessions);
        AnsiConsole.Write(table);
        
        var confirm = AnsiConsole.Confirm("Would you like to save the report as PDF?");

        if (confirm)
        {
            var pdfDocument = _reportGenerator.GenerateReportToFile(studySessions);
            _reportGenerator.SaveFullReportToPdf(pdfDocument);
        }
        GeneralHelperService.ShowContinueMessage();
    }
}