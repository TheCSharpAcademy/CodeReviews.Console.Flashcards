USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[StackNameById] Script Date: 28/12/2023 15:44:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[StackNameById]
	@param1 int = 0
AS
	SELECT Name FROM Stacks WHERE StackId = @param1
RETURN
