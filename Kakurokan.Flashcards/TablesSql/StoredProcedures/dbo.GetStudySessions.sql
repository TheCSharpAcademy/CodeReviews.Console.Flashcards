USE [AllStacks]
GO

/****** Object: SqlProcedure [dbo].[GetStudySessions] Script Date: 28/12/2023 15:44:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetStudySessions]
AS
	SELECT *
	FROM StudySessions
