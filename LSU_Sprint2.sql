
/*
LSU_Sprint2.sql
SQL Server T-SQL script to create and seed the Lynn Smith University Student Management database.
Tested for SQL Server LocalDB / SQL Server Express.

How to use:
1) Open SQL Server Management Studio (SSMS) and connect to (localdb)\MSSQLLocalDB (or your SQL instance).
2) File -> Open -> File... and select this .sql file.
3) Click Execute (F5). It will create database LSU_Sprint2, tables, and seed data.
*/

SET NOCOUNT ON;

IF DB_ID('LSU_Sprint2') IS NULL
BEGIN
    PRINT 'Creating database LSU_Sprint2...';
    CREATE DATABASE LSU_Sprint2;
END
GO

USE LSU_Sprint2;
GO

/* Drop existing tables (optional clean reset) */
IF OBJECT_ID('dbo.TeachingAssignment', 'U') IS NOT NULL DROP TABLE dbo.TeachingAssignment;
IF OBJECT_ID('dbo.CompletedCourse', 'U') IS NOT NULL DROP TABLE dbo.CompletedCourse;
IF OBJECT_ID('dbo.Enrollment', 'U') IS NOT NULL DROP TABLE dbo.Enrollment;
IF OBJECT_ID('dbo.Course', 'U') IS NOT NULL DROP TABLE dbo.Course;
IF OBJECT_ID('dbo.Applicant', 'U') IS NOT NULL DROP TABLE dbo.Applicant;
IF OBJECT_ID('dbo.[User]', 'U') IS NOT NULL DROP TABLE dbo.[User];
GO

/* Users (simple: Role as text) */
CREATE TABLE dbo.[User] (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Username      NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash  NVARCHAR(256) NOT NULL,
    Role          NVARCHAR(20) NOT NULL  -- 'Student' | 'Faculty' | 'Admin'
);
GO

/* Applicants (Admissions module) */
CREATE TABLE dbo.Applicant (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    FirstName     NVARCHAR(80) NOT NULL,
    LastName      NVARCHAR(80) NOT NULL,
    SubmittedAt   DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),
    Status        NVARCHAR(20)  NOT NULL DEFAULT 'Submitted' -- 'Submitted' | 'Accepted' | 'Rejected'
);
GO

/* Courses */
CREATE TABLE dbo.Course (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    Code           NVARCHAR(20) NOT NULL UNIQUE,       -- e.g., 'CSCI-101'
    Title          NVARCHAR(200) NOT NULL,
    Department     NVARCHAR(80) NULL,
    Capacity       INT NOT NULL DEFAULT 30,
    Enrolled       INT NOT NULL DEFAULT 0,
    Credits        INT NOT NULL DEFAULT 3,
    Prerequisites  NVARCHAR(200) NULL                  -- comma-separated course codes (simple approach)
);
GO

/* Enrollments: a student enrolled in a course */
CREATE TABLE dbo.Enrollment (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    StudentId     INT NOT NULL,
    CourseId      INT NOT NULL,
    EnrolledAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Enrollment_User FOREIGN KEY (StudentId) REFERENCES dbo.[User](Id) ON DELETE CASCADE,
    CONSTRAINT FK_Enrollment_Course FOREIGN KEY (CourseId) REFERENCES dbo.Course(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Enrollment UNIQUE (StudentId, CourseId)
);
GO

/* Completed Courses (for prerequisite checks) */
CREATE TABLE dbo.CompletedCourse (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    StudentId     INT NOT NULL,
    CourseId      INT NOT NULL,
    CompletedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Completed_User FOREIGN KEY (StudentId) REFERENCES dbo.[User](Id) ON DELETE CASCADE,
    CONSTRAINT FK_Completed_Course FOREIGN KEY (CourseId) REFERENCES dbo.Course(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Completed UNIQUE (StudentId, CourseId)
);
GO

/* Teaching Assignment (Admin assigns faculty to courses) */
CREATE TABLE dbo.TeachingAssignment (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    FacultyId  INT NOT NULL,
    CourseId   INT NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Teach_User FOREIGN KEY (FacultyId) REFERENCES dbo.[User](Id) ON DELETE CASCADE,
    CONSTRAINT FK_Teach_Course FOREIGN KEY (CourseId) REFERENCES dbo.Course(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Teach UNIQUE (FacultyId, CourseId)
);
GO

/* Seed Users (passwords are plain here; hash if you like) */
IF NOT EXISTS (SELECT 1 FROM dbo.[User])
BEGIN
    INSERT INTO dbo.[User] (Username, PasswordHash, Role)
    VALUES ('alice', 'Pass123$', 'Student'),
           ('bob',   'Pass123$', 'Faculty'),
           ('admin', 'Admin#2025', 'Admin');
END
GO

/* Seed Courses */
IF NOT EXISTS (SELECT 1 FROM dbo.Course)
BEGIN
    INSERT INTO dbo.Course (Code, Title, Department, Capacity, Enrolled, Credits, Prerequisites)
    VALUES ('CSCI-101', 'Intro to Programming', 'CS', 40, 0, 3, NULL),
           ('CSCI-201', 'Data Structures',      'CS', 35, 0, 3, 'CSCI-101'),
           ('MATH-121', 'Calculus I',            'Math', 40, 0, 4, NULL);
END
GO

/* Seed CompletedCourse: give Alice credit for CSCI-101 */
DECLARE @aliceId INT = (SELECT Id FROM dbo.[User] WHERE Username='alice');
DECLARE @cs101Id INT = (SELECT Id FROM dbo.Course WHERE Code='CSCI-101');
IF @aliceId IS NOT NULL AND @cs101Id IS NOT NULL
    AND NOT EXISTS (SELECT 1 FROM dbo.CompletedCourse WHERE StudentId=@aliceId AND CourseId=@cs101Id)
BEGIN
    INSERT INTO dbo.CompletedCourse (StudentId, CourseId) VALUES (@aliceId, @cs101Id);
END
GO

PRINT 'LSU_Sprint2 database is created and seeded.';
