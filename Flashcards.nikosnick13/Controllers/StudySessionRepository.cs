using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using FlashStudy.Models;
using System.Configuration;
using Flashcards.nikosnick13.DTOs;
using Microsoft.Data.SqlClient;

namespace FlashStudy.Data;

public static class StudySessionRepository
{
    private static string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    public static void AddStudySession(StudySession session)
    {
        using var conn = new SqlConnection(connectionString);
        conn.Open();

        string query = "INSERT INTO StudySessions (Score, Date, Stack_Id) VALUES (@Score, @Date, @StackId)";
        conn.Execute(query, session);
    }

    public static List<BasicFlashcardDTO> StudySessions
    {
        get
        {
            using var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = @"
                SELECT ss.Id, ss.Score, ss.Date, s.Name AS StackName
                FROM StudySessions ss
                JOIN Stacks s ON ss.Stack_Id = s.Id
                ORDER BY ss.Date DESC";

            return conn.Query<BasicFlashcardDTO>(query).AsList();
        }
    }
}
