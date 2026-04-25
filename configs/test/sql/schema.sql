USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CompanyDB')
BEGIN
    CREATE DATABASE CompanyDB;
END
GO

USE CompanyDB;
GO

-- Drop existing objects if they exist (for clean setup)
IF OBJECT_ID('sp_SearchEmployees', 'P') IS NOT NULL
    DROP PROCEDURE sp_SearchEmployees;
GO

IF OBJECT_ID('vw_EmployeesForExport', 'V') IS NOT NULL
    DROP VIEW vw_EmployeesForExport;
GO

IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

-- ================================================
-- Create Employees Table
-- ================================================
CREATE TABLE Employees (
    EmployeeID      INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(100) NOT NULL,
    Position        NVARCHAR(50)  NOT NULL,
    Department      NVARCHAR(50)  NOT NULL,
    HireDate        DATE          NOT NULL,
    Email           NVARCHAR(100) NULL,
    Phone           NVARCHAR(50)  NULL,
    Salary          DECIMAL(10,2) NULL,
    FileBlob        VARBINARY(MAX) NULL,
    FilePath        NVARCHAR(260) NULL,
    CreatedAt       DATETIME      NOT NULL DEFAULT GETDATE()
);
GO

CREATE NONCLUSTERED INDEX IX_Employees_FullName 
ON Employees(FullName);
GO

CREATE PROCEDURE sp_SearchEmployees
    @term NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        EmployeeID,
        FullName,
        Position,
        Department,
        HireDate,
        Email,
        Phone,
        Salary,
        FileBlob,
        FilePath,
        CreatedAt
    FROM 
        Employees
    WHERE 
        (@term IS NULL OR @term = '' OR
         FullName LIKE '%' + @term + '%' OR
         Position LIKE '%' + @term + '%' OR
         Department LIKE '%' + @term + '%' OR
         Email LIKE '%' + @term + '%' OR
         Phone LIKE '%' + @term + '%')
    ORDER BY 
        EmployeeID DESC;
END
GO

CREATE VIEW vw_EmployeesForExport AS
SELECT 
    EmployeeID,
    FullName,
    Position,
    Department,
    HireDate,
    Email,
    Phone,
    Salary
FROM 
    Employees;
GO
