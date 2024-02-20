USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[GetFlashcards] Script Date: 28/12/2023 15:44:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetFlashcards]
AS
	SELECT * 
	FROM Flashcards
