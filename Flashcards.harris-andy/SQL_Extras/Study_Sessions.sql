SELECT TOP (1000) [Id]
      ,[date]
      ,[score]
      ,[StackId]
      ,[questions]
  FROM [FlashCards].[dbo].[study_sessions]

-- DBCC CHECKIDENT ('study_sessions', NORESEED);
-- DBCC CHECKIDENT ('study_sessions', RESEED, 4);

-- DELETE FROM study_sessions WHERE Id = 6;

