USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[GetStacks] Script Date: 28/12/2023 15:44:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStacks]
AS
	SELECT * 
	FROM Stacks
