USE KinostudioDB;
GO

IF OBJECT_ID('dbo.GetDirectorFullName', 'FN') IS NOT NULL
    DROP FUNCTION dbo.GetDirectorFullName;
GO
IF OBJECT_ID('dbo.GetFilmCountForGenre', 'FN') IS NOT NULL
    DROP FUNCTION dbo.GetFilmCountForGenre;
GO
IF OBJECT_ID('dbo.GetTopFilmsByProfit', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetTopFilmsByProfit;
GO
IF OBJECT_ID('dbo.UpdateDepartmentEmployeeCount', 'P') IS NOT NULL
    DROP PROCEDURE dbo.UpdateDepartmentEmployeeCount;
GO

CREATE FUNCTION dbo.GetDirectorFullName
(
    @directorId INT
)
RETURNS NVARCHAR(101)
AS
BEGIN
    DECLARE @fullName NVARCHAR(101);

    SELECT @fullName = FirstName + N' ' + LastName
    FROM dbo.Director
    WHERE DirectorID = @directorId;

    RETURN @fullName;
END
GO

CREATE FUNCTION dbo.GetFilmCountForGenre
(
    @genreId INT
)
RETURNS INT
AS
BEGIN
    DECLARE @filmCount INT;

    SELECT @filmCount = COUNT(FG.FilmID)
    FROM dbo.FilmGenre AS FG
    WHERE FG.GenreID = @genreId;

    RETURN @filmCount;
END
GO

CREATE PROCEDURE dbo.GetTopFilmsByProfit
    @minProfit DECIMAL(18, 2),
    @year INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @minValue DECIMAL(18, 2);
    SET @minValue = ISNULL(@minProfit, 0);
    IF @year IS NOT NULL AND @year < 1800 SET @year = NULL;

    SELECT F.FilmID,
           F.Title,
           F.ReleaseYear,
           (ISNULL(F.BoxOffice, 0) - ISNULL(F.Budget, 0)) AS Profit
    FROM dbo.Film AS F
    WHERE (@year IS NULL OR F.ReleaseYear = @year)
      AND (ISNULL(F.BoxOffice, 0) - ISNULL(F.Budget, 0)) >= @minValue
    ORDER BY Profit DESC, F.Title;
END
GO

CREATE PROCEDURE dbo.UpdateDepartmentEmployeeCount
    @departmentId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @exists INT;
    DECLARE @employeeCount INT;
    DECLARE @studioId INT;
    DECLARE @studioCount INT;

    SELECT @exists = COUNT(1)
    FROM dbo.Department
    WHERE DepartmentID = @departmentId;

    IF @exists = 0
        RETURN;

    SELECT @employeeCount = COUNT(1)
    FROM dbo.Employee
    WHERE DepartmentID = @departmentId;

    UPDATE dbo.Department
    SET EmployeeCount = @employeeCount
    WHERE DepartmentID = @departmentId;

    SELECT @studioId = StudioID
    FROM dbo.Department
    WHERE DepartmentID = @departmentId;

    SELECT @studioCount = COUNT(1)
    FROM dbo.Employee AS E
    INNER JOIN dbo.Department AS D ON D.DepartmentID = E.DepartmentID
    WHERE D.StudioID = @studioId;

    UPDATE dbo.Studio
    SET EmployeeCount = @studioCount
    WHERE StudioID = @studioId;
END
GO
