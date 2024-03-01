using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.SamGannon.DTOs
{
    internal class StudyHistoryDto
    {
        public string? StackName { get; set; } = string.Empty;
        public TimeSpan StudyLength { get; set; }
        public decimal? Score {  get; set; }   

        public StudyHistoryDto() { }

        public StudyHistoryDto(StudyHistory history)
        {
            StackName = history.StackName;
            StudyLength = history.EndTime - history.StartTime;
            Score = history.Score;
        }

        public static List<StudyHistoryDto> ToDto(List<StudyHistory> reports)
        {
            List<StudyHistoryDto> lstReports = new List<StudyHistoryDto>();

            foreach (var report in reports)
            {
                var StudyHistoryDto = new StudyHistoryDto(report);
                lstReports.Add(StudyHistoryDto);
            }

            return lstReports;
        }
    }
}
