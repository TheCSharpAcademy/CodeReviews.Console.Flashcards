USE [AllStacks]
GO

/****** Object: Table [dbo].[Flashcards] Script Date: 09/01/2024 17:56:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Flashcards] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [StackId]  INT            NOT NULL,
    [Question] NVARCHAR (MAX) NOT NULL,
    [Answer]   NVARCHAR (MAX) NOT NULL
);


