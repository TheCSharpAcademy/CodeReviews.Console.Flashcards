USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[DeleteFlashcard] Script Date: 28/12/2023 15:43:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteFlashcard]
	@Id int
AS
	DELETE FROM Flashcards WHERE Id=@Id;
