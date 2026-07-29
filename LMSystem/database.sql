/*
 Digital Library Hub
 Optional SQL inspection and reset script.

 The application creates the DigitalLibraryHub database, ASP.NET Core Identity tables,
 application tables and sample data automatically on first run with EF Core EnsureCreated().
 You do not need to execute this file to run the project.
*/

-- Inspect the automatically created application database.
USE DigitalLibraryHub;
GO

SELECT * FROM Books13 ORDER BY BookId;
SELECT * FROM BorrowRecords13 ORDER BY BorrowRecordId DESC;
SELECT * FROM Students ORDER BY StudentId;
SELECT * FROM Librarians ORDER BY LibrarianId;
SELECT * FROM Publications ORDER BY [Type], PublishedDate DESC;
SELECT * FROM ContactMessages ORDER BY SubmittedAt DESC;

-- ASP.NET Core Identity inspection.
SELECT * FROM AspNetUsers;
SELECT * FROM AspNetRoles;
SELECT * FROM AspNetUserRoles;

-- Uncomment only when you deliberately want a clean database on the next application run.
-- USE master;
-- ALTER DATABASE DigitalLibraryHub SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- DROP DATABASE DigitalLibraryHub;
