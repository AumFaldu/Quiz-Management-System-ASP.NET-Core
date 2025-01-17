CREATE DATABASE EN_23010101076
CREATE TABLE MST_User (
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

CREATE TABLE MST_Question (
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

CREATE TABLE MST_Quiz (
    QuizID INT PRIMARY KEY IDENTITY(1,1),
    QuizName NVARCHAR(100) NOT NULL,
    TotalQuestions INT NOT NULL,
    QuizDate DATETIME NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(UserID) REFERENCES MST_User(UserID)
);

CREATE TABLE MST_QuestionLevel (
    QuestionLevelID INT PRIMARY KEY IDENTITY(1,1),
    QuestionLevel NVARCHAR(100) NOT NULL,
    UserID INT NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
	FOREIGN KEY(UserID) REFERENCES MST_User(UserID)
);

CREATE TABLE MST_QuizWiseQuestions (
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
    SELECT * FROM [dbo].[MST_User]
    ORDER BY UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_SelectByPK]
@UserID INT AS
BEGIN
    SELECT * FROM [dbo].[MST_User]
    WHERE UserID = @UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_Insert]
@UserName NVARCHAR(100),
@Password NVARCHAR(100),
@Email NVARCHAR(100),
@Mobile NVARCHAR(100),
@IsActive BIT = 1,
@IsAdmin BIT = 0,
@Created DATETIME = GETDATE,
@Modified DATETIME
AS
BEGIN
    INSERT INTO [dbo].[MST_User] (UserName, Password, Email, Mobile, IsActive, IsAdmin, Created, Modified)
    VALUES (@UserName, @Password, @Email, @Mobile, @IsActive, @IsAdmin, @Created, @Modified);
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_UpdateByPK]
@UserID INT,
@UserName NVARCHAR(100),
@Password NVARCHAR(100),
@Email NVARCHAR(100),
@Mobile NVARCHAR(100),
@IsActive BIT,
@IsAdmin BIT,
@Created DATETIME,
@Modified DATETIME
AS
BEGIN
    UPDATE [dbo].[MST_User]
    SET UserName = @UserName,
        Password = @Password,
        Email = @Email,
        Mobile = @Mobile,
        IsActive = @IsActive,
        IsAdmin = @IsAdmin,
        Created = @Created,
        Modified = @Modified
    WHERE UserID = @UserID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_User_DeleteByPK]
@UserID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_User]
    WHERE UserID = @UserID;
END
--MST_Question Table-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_SelectAll] AS
BEGIN
    SELECT Q.*, L.QuestionLevel, U.UserName FROM [dbo].[MST_Question] Q INNER JOIN [dbo].[MST_QuestionLevel] L
	ON Q.QuestionLevelID = L.QuestionLevelID INNER JOIN [dbo].[MST_User] U
	ON Q.UserID = U.UserID ORDER BY QuestionID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_SelectByPK]
@QuestionID INT AS
BEGIN
    SELECT * FROM [dbo].[MST_Question]
    WHERE QuestionID = @QuestionID;
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
@UserID INT,
@Created DATETIME = GETDATE,
@Modified DATETIME
AS
BEGIN
    INSERT INTO [dbo].[MST_Question] (QuestionText, QuestionLevelID, OptionA, OptionB, OptionC, OptionD, CorrectOption, QuestionMarks, IsActive, UserID, Created, Modified)
    VALUES (@QuestionText, @QuestionLevelID, @OptionA, @OptionB, @OptionC, @OptionD, @CorrectOption, @QuestionMarks, @IsActive, @UserID, @Created, @Modified);
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
@UserID INT,
@Created DATETIME,
@Modified DATETIME
AS
BEGIN
    UPDATE [dbo].[MST_Question]
    SET QuestionText = @QuestionText,
        QuestionLevelID = @QuestionLevelID,
        OptionA = @OptionA,
        OptionB = @OptionB,
        OptionC = @OptionC,
        OptionD = @OptionD,
        CorrectOption = @CorrectOption,
        QuestionMarks = @QuestionMarks,
        IsActive = @IsActive,
        UserID = @UserID,
        Created = @Created,
        Modified = @Modified
    WHERE QuestionID = @QuestionID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Question_DeleteByPK]
@QuestionID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_Question]
    WHERE QuestionID = @QuestionID;
END
--MST_Quiz TABLE-SP 
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_SelectAll] AS
BEGIN
    SELECT Q.*, U.UserName FROM [dbo].[MST_Quiz] Q INNER JOIN [dbo].[MST_User] U
	ON Q.UserID = U.UserID ORDER BY QuizID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_SelectByPK]
@QuizID INT AS
BEGIN
    SELECT * FROM [dbo].[MST_Quiz]
    WHERE QuizID = @QuizID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_Insert]
@QuizName NVARCHAR(100),
@TotalQuestions INT,
@QuizDate DATETIME,
@UserID INT,
@Created DATETIME = GETDATE,
@Modified DATETIME
AS
BEGIN
    INSERT INTO [dbo].[MST_Quiz] (QuizName, TotalQuestions, QuizDate, UserID, Created, Modified)
    VALUES (@QuizName, @TotalQuestions, @QuizDate, @UserID, @Created, @Modified);
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_UpdateByPK]
@QuizID INT,
@QuizName NVARCHAR(100),
@TotalQuestions INT,
@QuizDate DATETIME,
@UserID INT,
@Created DATETIME,
@Modified DATETIME
AS
BEGIN
    UPDATE [dbo].[MST_Quiz]
    SET QuizName = @QuizName,
        TotalQuestions = @TotalQuestions,
        QuizDate = @QuizDate,
        UserID = @UserID,
        Created = @Created,
        Modified = @Modified
    WHERE QuizID = @QuizID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_Quiz_DeleteByPK]
@QuizID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_Quiz]
    WHERE QuizID = @QuizID;
END
--MST_QuestionLevel TABLE-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectAll] AS
BEGIN
    SELECT L.*, U.UserName
    FROM [dbo].[MST_QuestionLevel] L
    INNER JOIN [dbo].[MST_User] U ON L.UserID = U.UserID
    ORDER BY QuestionLevelID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_SelectByPK]
@QuestionLevelID INT AS
BEGIN
    SELECT * FROM [dbo].[MST_QuestionLevel]
    WHERE QuestionLevelID = @QuestionLevelID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_Insert]
@QuestionLevel NVARCHAR(100),
@UserID INT,
@Created DATETIME = GETDATE,
@Modified DATETIME
AS
BEGIN
    INSERT INTO [dbo].[MST_QuestionLevel] (QuestionLevel, UserID, Created, Modified)
    VALUES (@QuestionLevel, @UserID, @Created, @Modified);
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_UpdateByPK]
@QuestionLevelID INT,
@QuestionLevel NVARCHAR(100),
@UserID INT,
@Created DATETIME,
@Modified DATETIME
AS
BEGIN
    UPDATE [dbo].[MST_QuestionLevel]
    SET QuestionLevel = @QuestionLevel,
        UserID = @UserID,
        Created = @Created,
        Modified = @Modified
    WHERE QuestionLevelID = @QuestionLevelID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuestionLevel_DeleteByPK]
@QuestionLevelID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_QuestionLevel]
    WHERE QuestionLevelID = @QuestionLevelID;
END
--MST_QuizWiseQuestions TABLE-SP
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectAll] AS
BEGIN
    SELECT QWQ.*, Q.QuizName, QN.QuestionText, U.UserName
    FROM [dbo].[MST_QuizWiseQuestions] QWQ
    INNER JOIN [dbo].[MST_Quiz] Q ON QWQ.QuizID = Q.QuizID
    INNER JOIN [dbo].[MST_Question] QN ON QWQ.QuestionID = QN.QuestionID
    INNER JOIN [dbo].[MST_User] U ON QWQ.UserID = U.UserID
    ORDER BY QuizWiseQuestionsID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_SelectByPK]
@QuizWiseQuestionsID INT AS
BEGIN
    SELECT QWQ.*, Q.QuizName, QN.QuestionText, U.UserName
    FROM [dbo].[MST_QuizWiseQuestions] QWQ
    INNER JOIN [dbo].[MST_Quiz] Q ON QWQ.QuizID = Q.QuizID
    INNER JOIN [dbo].[MST_Question] QN ON QWQ.QuestionID = QN.QuestionID
    INNER JOIN [dbo].[MST_User] U ON QWQ.UserID = U.UserID
    WHERE QWQ.QuizWiseQuestionsID = @QuizWiseQuestionsID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_Insert]
@QuizID INT,
@QuestionID INT,
@UserID INT,
@Created DATETIME = GETDATE,
@Modified DATETIME
AS
BEGIN
    INSERT INTO [dbo].[MST_QuizWiseQuestions] (QuizID, QuestionID, UserID, Created, Modified)
    VALUES (@QuizID, @QuestionID, @UserID, @Created, @Modified);
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_UpdateByPK]
@QuizWiseQuestionsID INT,
@QuizID INT,
@QuestionID INT,
@UserID INT,
@Created DATETIME,
@Modified DATETIME
AS
BEGIN
    UPDATE [dbo].[MST_QuizWiseQuestions]
    SET QuizID = @QuizID,
        QuestionID = @QuestionID,
        UserID = @UserID,
        Created = @Created,
        Modified = @Modified
    WHERE QuizWiseQuestionsID = @QuizWiseQuestionsID;
END
CREATE OR ALTER PROCEDURE [dbo].[PR_MST_QuizWiseQuestions_DeleteByPK]
@QuizWiseQuestionsID INT
AS
BEGIN
    DELETE FROM [dbo].[MST_QuizWiseQuestions]
    WHERE QuizWiseQuestionsID = @QuizWiseQuestionsID;
END