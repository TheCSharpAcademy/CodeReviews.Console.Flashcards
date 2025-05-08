CREATE TABLE [dbo].[StudySession](
	[ID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Date] [date] NOT NULL,
	[Score] [float] NOT NULL,
	[StackID] [int] NOT NULL FOREIGN KEY REFERENCES Stack(ID) ON DELETE CASCADE
)