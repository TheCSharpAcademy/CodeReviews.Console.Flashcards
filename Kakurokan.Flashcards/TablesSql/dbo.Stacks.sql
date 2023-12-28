USE [AllStacks]
GO

/****** Object: Table [dbo].[Stacks] Script Date: 28/12/2023 15:43:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Stacks] (
    [StackId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL
);


