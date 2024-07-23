﻿CREATE TABLE [dbo].[Stack]
(
	 [Id] INT NOT NULL IDENTITY
    ,[Name] NVARCHAR(64) NOT NULL
    ,CONSTRAINT [PK_Stack] PRIMARY KEY ([Id])
    ,CONSTRAINT [UQ_Stack_Name] UNIQUE ([Name])
)
GO
