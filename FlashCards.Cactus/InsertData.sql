Use flashcardsdb;

-- Insert rows into table 'Stack'
INSERT INTO flashcardsdb.dbo.Stack(name)
VALUES
('English'),
('Algorithm')
GO

-- Insert rows into table 'FlashCard'
INSERT INTO flashcardsdb.dbo.FlashCard (front, back, sid)
VALUES
('Human Rights', N'Ren Quan', 1),
('Freedom', N'Zi You', 1),
('Democracy', N'Min Zhu', 1),
('1 + 1 = ', '2', 2)
GO

-- Insert rows into table 'StudySession'
INSERT INTO flashcardsdb.dbo.StudySession
(sid, date, score)
VALUES
(1, GETDATE(), 2)
GO

-- Insert rows into table 'StudySession'
INSERT INTO flashcardsdb.dbo.StudySession
(sid, date, score)
VALUES
(2, GETDATE(), 1)
GO