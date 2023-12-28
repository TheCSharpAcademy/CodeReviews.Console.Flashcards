USE [AllStacks]
GO

/****** Object: Table [dbo].[StudySessions] Script Date: 28/12/2023 15:43:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StudySessions] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Score]   INT           NULL,
    [Date]    NVARCHAR (50) NOT NULL,
    [StackId] INT           NOT NULL
);


