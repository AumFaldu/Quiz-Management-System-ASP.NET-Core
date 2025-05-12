CREATE TABLE [dbo].[MST_User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Mobile NVARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsAdmin BIT NOT NULL DEFAULT 0,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);

CREATE TABLE [dbo].[MST_Question] (
    QuestionID INT PRIMARY KEY IDENTITY(1,1),
    QuestionText NVARCHAR(MAX) NOT NULL,
    QuestionLevelID INT NOT NULL,
    OptionA NVARCHAR(100) NOT NULL,
    OptionB NVARCHAR(100) NOT NULL,
    OptionC NVARCHAR(100) NOT NULL,
    OptionD NVARCHAR(100) NOT NULL,
    CorrectOption NVARCHAR(100) NOT NULL,
    QuestionMarks INT NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    UserID INT NOT NULL,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(QuestionLevelID) REFERENCES MST_QuestionLevel(QuestionLevelID)
);
CREATE TABLE [dbo].[MST_Quiz] (
    QuizID INT PRIMARY KEY IDENTITY(1,1),
    QuizName NVARCHAR(100) NOT NULL,
    TotalQuestions INT NOT NULL,
    QuizDate DATETIME NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(UserID) REFERENCES MST_User(UserID)
);

CREATE TABLE [dbo].[MST_QuestionLevel] (
    QuestionLevelID INT PRIMARY KEY IDENTITY(1,1),
    QuestionLevel NVARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(UserID) REFERENCES MST_User(UserID)
);

CREATE TABLE [dbo].[MST_QuizWiseQuestions] (
    QuizWiseQuestionsID INT PRIMARY KEY IDENTITY(1,1),
    QuizID INT NOT NULL,
    QuestionID INT NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(QuizID) REFERENCES MST_Quiz(QuizID),
	FOREIGN KEY(QuestionID) REFERENCES MST_Question(QuestionID),
	FOREIGN KEY(UserID) REFERENCES MST_User(UserID)
);
--MST_User Table-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_SelectAll]
AS
BEGIN
    SELECT [dbo].[MST_User].[UserID],[dbo].[MST_User].[UserName],[dbo].[MST_User].[Password],[dbo].[MST_User].[Email],[dbo].[MST_User].[Mobile],[dbo].[MST_User].[IsActive],[dbo].[MST_User].[IsAdmin],[dbo].[MST_User].[Created],[dbo].[MST_User].[Modified] FROM [dbo].[MST_User]
    ORDER BY UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_SelectByPK]
@UserID INT AS
BEGIN
    SELECT [dbo].[MST_User].[UserID],[dbo].[MST_User].[UserName],[dbo].[MST_User].[Password],[dbo].[MST_User].[Email],[dbo].[MST_User].[Mobile],[dbo].[MST_User].[IsActive],[dbo].[MST_User].[IsAdmin],[dbo].[MST_User].[Created],[dbo].[MST_User].[Modified] FROM [dbo].[MST_User]
    WHERE [dbo].[MST_User].[UserID] = @UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_Insert]
@UserName NVARCHAR(100),
@Password NVARCHAR(100),
@Email NVARCHAR(100),
@Mobile NVARCHAR(100)
AS
BEGIN
    INSERT INTO [dbo].[MST_User] ([UserName], [Password], [Email], [Mobile], [Modified])
    VALUES (@UserName, @Password, @Email, @Mobile, GETDATE());
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_UpdateByPK]
@UserID INT,
@UserName NVARCHAR(100),
@Password NVARCHAR(100),
@Email NVARCHAR(100),
@Mobile NVARCHAR(100)
AS
BEGIN
    UPDATE [dbo].[MST_User]
    SET [UserName] = @UserName,
        [Password] = @Password,
        [Email] = @Email,
        [Mobile] = @Mobile
    WHERE UserID = @UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_DeleteByPK]
@UserID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_User]
    WHERE [dbo].[MST_User].[UserID] = @UserID;
END
--MST_Question Table-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_SelectAll] AS
BEGIN
    SELECT [dbo].[MST_Question].[QuestionID],[dbo].[MST_Question].[QuestionText],[dbo].[MST_QuestionLevel].[QuestionLevelID],[dbo].[MST_Question].[OptionA],[dbo].[MST_Question].[OptionB],[dbo].[MST_Question].[OptionC],[dbo].[MST_Question].[OptionD],[dbo].[MST_Question].[CorrectOption],[dbo].[MST_Question].[QuestionMarks],[dbo].[MST_Question].[IsActive],[dbo].[MST_User].[UserID],[dbo].[MST_Question].[Created],[dbo].[MST_Question].[Modified] FROM [dbo].[MST_Question] INNER JOIN [dbo].[MST_QuestionLevel]
	ON [dbo].[MST_QuestionLevel].[QuestionLevelID] = [dbo].[MST_Question].[QuestionLevelID] INNER JOIN [dbo].[MST_User]
	ON [dbo].[MST_User].[UserID] = [dbo].[MST_Question].[UserID] 
	ORDER BY [dbo].[MST_Question].[QuestionID],[dbo].[MST_User].[UserID],[dbo].[MST_QuestionLevel].[QuestionLevelID];
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_SelectByPK]
@QuestionID INT AS
BEGIN
    SELECT [dbo].[MST_Question].[QuestionID],[dbo].[MST_Question].[QuestionText],[dbo].[MST_QuestionLevel].[QuestionLevelID],[dbo].[MST_Question].[OptionA],[dbo].[MST_Question].[OptionB],[dbo].[MST_Question].[OptionC],[dbo].[MST_Question].[OptionD],[dbo].[MST_Question].[CorrectOption],[dbo].[MST_Question].[QuestionMarks],[dbo].[MST_Question].[IsActive],[dbo].[MST_User].[UserID],[dbo].[MST_Question].[Created],[dbo].[MST_Question].[Modified] FROM [dbo].[MST_Question] INNER JOIN [dbo].[MST_QuestionLevel]
	ON [dbo].[MST_QuestionLevel].[QuestionLevelID] = [dbo].[MST_Question].[QuestionLevelID] INNER JOIN [dbo].[MST_User]
	ON [dbo].[MST_User].[UserID] = [dbo].[MST_Question].[UserID] 
    WHERE [dbo].[MST_Question].[QuestionID] = @QuestionID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_Insert]
@QuestionText NVARCHAR(MAX),
@QuestionLevelID INT,
@OptionA NVARCHAR(100),
@OptionB NVARCHAR(100),
@OptionC NVARCHAR(100),
@OptionD NVARCHAR(100),
@CorrectOption NVARCHAR(100),
@QuestionMarks INT,
@IsActive BIT = 1,
@UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_Question] ([QuestionText], [QuestionLevelID], [OptionA], [OptionB], [OptionC], [OptionD], [CorrectOption], [QuestionMarks], [IsActive], [UserID],[Modified])
    VALUES (@QuestionText, @QuestionLevelID, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption, @QuestionMarks, @IsActive, @UserID,GETDATE());
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_UpdateByPK]
@QuestionID INT,
@QuestionText NVARCHAR(MAX),
@QuestionLevelID INT,
@OptionA NVARCHAR(100),
@OptionB NVARCHAR(100),
@OptionC NVARCHAR(100),
@OptionD NVARCHAR(100),
@CorrectOption NVARCHAR(100),
@QuestionMarks INT,
@IsActive BIT,
@UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_Question]
    SET [QuestionText] = @QuestionText,
        [QuestionLevelID] = @QuestionLevelID,
        [OptionA] = @OptionA,
        [OptionB] = @OptionB,
        [OptionC] = @OptionC,
        [OptionD] = @OptionD,
        [CorrectOption] = @CorrectOption,
        [QuestionMarks] = @QuestionMarks,
        [IsActive] = @IsActive,
        [UserID] = @UserID
    WHERE [dbo].[MST_Question].[QuestionID] = @QuestionID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_DeleteByPK]
@QuestionID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_Question]
    WHERE [dbo].[MST_Question].[QuestionID] = @QuestionID;
END
--MST_Quiz TABLE-SP 
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_SelectAll] AS
BEGIN
    SELECT [dbo].[MST_Quiz].[QuizID],
	[dbo].[MST_Quiz].[QuizName],[dbo].[MST_Quiz].[TotalQuestions],[dbo].[MST_Quiz].[QuizDate],[dbo].[MST_User].[UserID],[dbo].[MST_Quiz].[Created],[dbo].[MST_Quiz].[Modified] FROM [dbo].[MST_Quiz] INNER JOIN [dbo].[MST_User]
	ON [dbo].[MST_User].[UserID] = [dbo].[MST_Quiz].[UserID]
	ORDER BY [dbo].[MST_Quiz].[QuizID],[dbo].[MST_User].[UserID]
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_SelectByPK]
@QuizID INT AS
BEGIN
    SELECT [dbo].[MST_Quiz].[QuizID],
		   [dbo].[MST_Quiz].[QuizName],
		   [dbo].[MST_Quiz].[TotalQuestions],
		   [dbo].[MST_Quiz].[QuizDate],
		   [dbo].[MST_User].[UserID],
		   [dbo].[MST_Quiz].[Created],
		   [dbo].[MST_Quiz].[Modified] 
	FROM [dbo].[MST_Quiz] 
	INNER JOIN [dbo].[MST_User]
	ON [dbo].[MST_User].[UserID] = [dbo].[MST_Quiz].[UserID]
    WHERE [dbo].[MST_Quiz].[QuizID] = @QuizID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_Insert]
@QuizName NVARCHAR(100),
@TotalQuestions INT,
@QuizDate DATETIME,
@UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_Quiz] ([QuizName], [TotalQuestions], [QuizDate], [UserID],[Modified])
    VALUES (@QuizName, @TotalQuestions, @QuizDate, @UserID,GETDATE());
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_UpdateByPK]
@QuizID INT,
@QuizName NVARCHAR(100),
@TotalQuestions INT,
@QuizDate DATETIME,
@UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_Quiz]
    SET [QuizName] = @QuizName,
        [TotalQuestions] = @TotalQuestions,
        [QuizDate] = @QuizDate,
        [UserID] = @UserID
    WHERE [dbo].[MST_Quiz].[QuizID] = @QuizID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_DeleteByPK]
@QuizID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_Quiz]
    WHERE [dbo].[MST_Quiz].[QuizID] = @QuizID;
END
CREATE OR ALTER PROCEDURE PR_MST_User_SelectByUserNamePassword
@UserName varchar(50),
@Password varchar(50)
AS
BEGIN
	SELECT [dbo].[MST_User].[USERNAME],[dbo].[MST_User].[PASSWORD] FROM [dbo].[MST_USER]
	WHERE (UserName=@UserName OR Email=@UserName OR Mobile=@UserName) and Password=@Password and isActive=1
END
--MST_QuestionLevel TABLE-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectAll] AS
BEGIN
    SELECT [dbo].[MST_QuestionLevel].[QuestionLevelID],[dbo].[MST_QuestionLevel].[QuestionLevel],[dbo].[MST_User].[UserID],[dbo].[MST_QuestionLevel].[Created],[dbo].[MST_QuestionLevel].[Modified]
    FROM [dbo].[MST_QuestionLevel]
    INNER JOIN [dbo].[MST_User] ON [dbo].[MST_User].[UserID] = [dbo].[MST_QuestionLevel].[UserID]
    ORDER BY [dbo].[MST_QuestionLevel].[QuestionLevelID],[dbo].[MST_User].[UserID]
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectByPK]
@QuestionLevelID INT AS
BEGIN
    SELECT [dbo].[MST_QuestionLevel].[QuestionLevelID],[dbo].[MST_QuestionLevel].[QuestionLevel],[dbo].[MST_User].[UserID],[dbo].[MST_QuestionLevel].[Created],[dbo].[MST_QuestionLevel].[Modified]
    FROM [dbo].[MST_QuestionLevel]
    INNER JOIN [dbo].[MST_User] ON [dbo].[MST_User].[UserID] = [dbo].[MST_QuestionLevel].[UserID]
    WHERE [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_Insert]
@QuestionLevel NVARCHAR(100),
@UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_QuestionLevel] ([QuestionLevel], [UserID],[Modified])
    VALUES (@QuestionLevel, @UserID,GETDATE());
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_UpdateByPK]
@QuestionLevelID INT,
@QuestionLevel NVARCHAR(100),
@UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_QuestionLevel]
    SET [QuestionLevel] = @QuestionLevel,
        [UserID] = @UserID
    WHERE [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_DeleteByPK]
@QuestionLevelID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_QuestionLevel]
    WHERE [dbo].[MST_QuestionLevel].[QuestionLevelID] = @QuestionLevelID
END
--MST_QuizWiseQuestions TABLE-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectAll] AS
BEGIN
    SELECT [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID],
	[dbo].[MST_Quiz].[QuizID],
	[dbo].[MST_Question].[QuestionID],
	[dbo].[MST_User].[UserID],
	[dbo].[MST_QuizWiseQuestions].[Created],
	[dbo].[MST_QuizWiseQuestions].[Modified],
	[dbo].[MST_Question].[Questiontext],
	[dbo].[MST_Question].[OptionA],
	[dbo].[MST_Question].[OptionB],
	[dbo].[MST_Question].[OptionC],
	[dbo].[MST_Question].[OptionD],
	[dbo].[MST_Question].[CorrectOption],
	[dbo].[MST_Question].[QuestionMarks],
	[dbo].[MST_Quiz].[QuizName],
	[dbo].[MST_Quiz].[TotalQuestions],
	[dbo].[MST_User].[UserName]
    FROM [dbo].[MST_QuizWiseQuestions]
    INNER JOIN [dbo].[MST_Quiz] ON [dbo].[MST_Quiz].[QuizID] = [dbo].[MST_QuizWiseQuestions].[QuizID]
    INNER JOIN [dbo].[MST_Question]  ON [dbo].[MST_Question].[QuestionID] = [dbo].[MST_QuizWiseQuestions].[QuestionID]
    INNER JOIN [dbo].[MST_User] ON [dbo].[MST_User].[UserID] = [dbo].[MST_QuizWiseQuestions].[UserID]
    ORDER BY [dbo].[MST_QuizWiseQuestions].[QuizID],[dbo].[MST_QuizWiseQuestions].[QuestionID],[dbo].[MST_QuizWiseQuestions].[UserID]
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectByPK]
@QuizWiseQuestionsID INT AS
BEGIN
    SELECT [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID],
	[dbo].[MST_Quiz].[QuizID],
	[dbo].[MST_Question].[QuestionID],
	[dbo].[MST_User].[UserID],
	[dbo].[MST_QuizWiseQuestions].[Created],
	[dbo].[MST_QuizWiseQuestions].[Modified],
	[dbo].[MST_Question].[Questiontext],
	[dbo].[MST_Question].[OptionA],
	[dbo].[MST_Question].[OptionB],
	[dbo].[MST_Question].[OptionC],
	[dbo].[MST_Question].[OptionD],
	[dbo].[MST_Question].[CorrectOption],
	[dbo].[MST_Question].[QuestionMarks],
	[dbo].[MST_Quiz].[QuizName],
	[dbo].[MST_Quiz].[TotalQuestions]
    FROM [dbo].[MST_QuizWiseQuestions]
    INNER JOIN [dbo].[MST_Quiz] ON [dbo].[MST_Quiz].[QuizID] = [dbo].[MST_QuizWiseQuestions].[QuizID]
    INNER JOIN [dbo].[MST_Question]  ON [dbo].[MST_Question].[QuestionID] = [dbo].[MST_QuizWiseQuestions].[QuestionID]
    INNER JOIN [dbo].[MST_User] ON [dbo].[MST_User].[UserID] = [dbo].[MST_QuizWiseQuestions].[UserID]
    WHERE [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_Insert]
@QuizID INT,
@QuestionID INT,
@UserID INT
AS
BEGIN
    INSERT INTO [dbo].[MST_QuizWiseQuestions] ([QuizID], [QuestionID], [UserID], [Modified])
    VALUES (@QuizID, @QuestionID, @UserID, GETDATE());
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_UpdateByPK]
@QuizWiseQuestionsID INT,
@QuizID INT,
@QuestionID INT,
@UserID INT
AS
BEGIN
    UPDATE [dbo].[MST_QuizWiseQuestions]
    SET [QuizID] = @QuizID,
        [QuestionID] = @QuestionID,
        [UserID] = @UserID
    WHERE [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_DeleteByPK]
@QuizWiseQuestionsID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_QuizWiseQuestions]
    WHERE [dbo].[MST_QuizWiseQuestions].[QuizWiseQuestionsID] = @QuizWiseQuestionsID;
END
--Login
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_ValidateLogin]
@UserNameEmail VARCHAR(20),
@Password VARCHAR(20)
AS
BEGIN
	SELECT [dbo].[MST_User].[UserID],
			[dbo].[MST_User].[UserName],
			[dbo].[MST_User].[Email]
	FROM [dbo].[MST_User]
	WHERE 
		([dbo].[MST_User].[UserName]=@UserNameEmail OR [dbo].[MST_User].[Email]=@UserNameEmail)
		AND [dbo].[MST_User].[Password]=@Password
END

--Insert Dummy Data
INSERT INTO MST_User (UserName, Password, Email, Mobile, IsActive, IsAdmin, Modified) 
VALUES 
('AdminUser2', 'admin1234', 'admin2@example.com', '9876543211', 1, 1, GETDATE()),
('JohnDoe', 'password123', 'john@example.com', '1234567890', 1, 0, GETDATE()),
('JaneSmith', 'jane123', 'jane@example.com', '1122334455', 1, 0, GETDATE());
-- Insert into MST_QuestionLevel
INSERT INTO MST_QuestionLevel (QuestionLevel, UserID, Modified) 
VALUES 
('Easy', 1, GETDATE()),
('Medium', 2, GETDATE()),
('Hard', 3, GETDATE());

-- Insert into MST_Question
INSERT INTO MST_Question (QuestionText, QuestionLevelID, OptionA, OptionB, OptionC, OptionD, CorrectOption, QuestionMarks, IsActive, UserID, Modified)
VALUES 
('What is the capital of France?', 2, 'Paris', 'London', 'Berlin', 'Madrid', 'A', 5, 1, 1, GETDATE()),
('What is 2 + 2?', 3, '3', '4', '5', '6', 'B', 1, 1, 2, GETDATE()),
('What is the square root of 64?', 4, '6', '7', '8', '9', 'C', 10, 10, 3, GETDATE())
SELECT * FROM MST_Question
SELECT * FROM MST_Quiz
SELECT * FROM MST_User
SELECT * FROM MST_QuestionLevel
-- Insert into MST_Quiz
INSERT INTO MST_Quiz (QuizName, TotalQuestions, QuizDate, UserID, Modified) 
VALUES 
('General Knowledge Quiz', 10, '2025-02-20 10:00:00', 1, GETDATE())
('Math Quiz', 5, '2025-02-25 15:00:00', 2, GETDATE()),
('Science Quiz', 8, '2025-02-28 12:00:00', 3, GETDATE());

-- Insert into MST_QuizWiseQuestions
INSERT INTO MST_QuizWiseQuestions (QuizID, QuestionID, UserID, Modified) 
VALUES 
(2, 6, 1, GETDATE()),
(3, 7, 2, GETDATE()),
(4, 8, 3, GETDATE())

SELECT * FROM MST_QuizWiseQuestions
SELECT * FROM MST_QuestionLevel
SELECT * FROM MST_Question
SELECT * FROM MST_Quiz
SELECT * FROM MST_User

ALTER TABLE MST_Quiz DROP CONSTRAINT FK__MST_Quiz__UserID__47DBAE45;

ALTER TABLE MST_Quiz
ADD CONSTRAINT FK_MST_Quiz_UserID FOREIGN KEY (UserID)
REFERENCES MST_User(UserID)
