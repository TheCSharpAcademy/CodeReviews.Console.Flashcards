﻿namespace Flashcards.AshtonLeeSeloka.Services;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Dapper;

internal class DataService
{
	private readonly string? _ConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
	private readonly string? _DBCreationString = ConfigurationManager.AppSettings.Get("DBCreationString");

	public void CreateDB()
	{
		try
		{
			using (SqlConnection connection = new SqlConnection(_DBCreationString))
			{
				connection.Open();
				var SQLCommand = connection.CreateCommand();
				SQLCommand.CommandText = @"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name ='FlashCardsDB')
															BEGIN
																CREATE DATABASE FlashCardsDB;
															END;";
				SQLCommand.ExecuteNonQuery();
				Console.WriteLine("Sucess");
				connection.Close();
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}
	}

	public void CreateTables() 
	{
		var connection = new SqlConnection(_ConnectionString);
		var SQLCommandStack = @"IF OBJECT_ID(N'dbo.stack',N'U') IS NULL
							CREATE TABLE stack(
							Stack_ID INT IDENTITY(1,1) PRIMARY KEY,
							Stack_Name VARCHAR(20) NOT NULL UNIQUE,
							);";
		connection.Execute(SQLCommandStack);

		var SQLCommandCard = @"IF OBJECT_ID(N'dbo.cards',N'U') IS NULL
							CREATE TABLE dbo.Cards (
							ID int Not Null,
							Front TEXT,
							Back TEXT,
							Stack_ID int Not Null,
							PRIMARY KEY (ID),
							FOREIGN KEY (Stack_ID) REFERENCES stack (Stack_ID)
							);";
		connection.Execute(SQLCommandCard);

		var SQLCommandStudy = @"IF OBJECT_ID(N'dbo.study_session',N'U') IS NULL
							CREATE TABLE study_session(
							ID INT IDENTITY(1,1) PRIMARY KEY,
							Stack TEXT NOT NULL,
							Study_date DATE,
							Score DECIMAL(3,1),
							Stack_ID INT NOT NULL,
							FOREIGN KEY (Stack_ID) REFERENCES stack (Stack_ID) 
							);";
		connection.Execute(SQLCommandStudy);
	}

	public void SeedTableStack() 
	{
		try
		{
			var connection = new SqlConnection(_ConnectionString);
			var SQLCommand = @"INSERT INTO stack(Stack_Name) VALUES('German'),('Spanish'),('Swahili');";
			connection.Execute(SQLCommand);
		}
		catch
		{
			Console.Write("Vales already seeded");
		}
	}

	public void SeedTablesCards() 
	{
	
	}
}
