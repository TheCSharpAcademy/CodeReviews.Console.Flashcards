CREATE TABLE [dbo].[Flashcard](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Front] [varchar](50) NOT NULL,
	[Back] [varchar](50) NULL,
	[StackID] [int] NOT NULL FOREIGN KEY REFERENCES Stack(ID) ON DELETE CASCADE
)