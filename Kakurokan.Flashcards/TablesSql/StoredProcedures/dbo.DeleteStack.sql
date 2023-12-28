USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[DeleteStack] Script Date: 28/12/2023 15:44:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteStack]
	@StackId int
AS
	DELETE FROM Flashcards WHERE StackId=@StackId;
	DELETE FROM StudySessions WHERE StackId=@StackId;
	DELETE FROM Stacks WHERE StackId=@StackId;
