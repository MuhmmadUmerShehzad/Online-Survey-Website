CREATE DATABASE SurveyDB;
GO

USE SurveyDB;
GO

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    UserPassword VARCHAR(100) NOT NULL,
    UserRole VARCHAR(20) CHECK (UserRole IN ('Admin','Builder','Surveyor')) NOT NULL
);

CREATE TABLE Surveys (
    SurveyID INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    CreatedBy INT,
    IsAnonymous BIT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID)
);

CREATE TABLE Questions (
    QuestionID INT IDENTITY(1,1) PRIMARY KEY,
    SurveyID INT NOT NULL,
    QuestionText VARCHAR(500) NOT NULL,
    QuestionType VARCHAR(20) CHECK (QuestionType IN ('MCQ','TrueFalse')) NOT NULL,

    FOREIGN KEY (SurveyID) REFERENCES Surveys(SurveyID) ON DELETE CASCADE
);
CREATE TABLE Options (
    OptionID INT IDENTITY(1,1) PRIMARY KEY,
    QuestionID INT NOT NULL,
    OptionText VARCHAR(200) NOT NULL,

    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID) ON DELETE CASCADE
);
CREATE TABLE Responses (
    ResponseID INT IDENTITY(1,1) PRIMARY KEY,
    SurveyID INT NOT NULL,
    UserID INT NULL,
    ResponseDate DATETIME DEFAULT GETDATE(),

    FOREIGN KEY (SurveyID) REFERENCES Surveys(SurveyID),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
CREATE TABLE Answers (
    AnswerID INT IDENTITY(1,1) PRIMARY KEY,
    ResponseID INT NOT NULL,
    QuestionID INT NOT NULL,
    SelectedOptionID INT NOT NULL,

    FOREIGN KEY (ResponseID) REFERENCES Responses(ResponseID) ON DELETE CASCADE,
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID),
    FOREIGN KEY (SelectedOptionID) REFERENCES Options(OptionID)
);
INSERT INTO Users (UserName, Email, UserPassword, UserRole)
VALUES 
('admin', 'admin@test.com', '123', 'Admin'),
('builder', 'builder@test.com', '123', 'Builder'),
('umer', 'user@test.com', '123', 'Surveyor');

SELECT * FROM Users
SELECT * FROM Surveys
SELECT * FROM Questions
SELECT * FROM Options
SELECT * FROM Responses
SELECT * FROM Answers