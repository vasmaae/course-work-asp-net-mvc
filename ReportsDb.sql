USE KinostudioDB;
GO

IF OBJECT_ID('dbo.GetDirectorFullName', 'FN') IS NOT NULL
    DROP FUNCTION dbo.GetDirectorFullName;
GO
IF OBJECT_ID('dbo.GetFilmCountForGenre', 'FN') IS NOT NULL
    DROP FUNCTION dbo.GetFilmCountForGenre;
GO
IF OBJECT_ID('dbo.GetTopFilmsByRating', 'P') IS NOT NULL
    DROP PROCEDURE dbo.GetTopFilmsByRating;
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

CREATE PROCEDURE dbo.GetTopFilmsByRating
    @minRating DECIMAL(3, 1)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT F.FilmID,
           F.Title,
           F.ReleaseYear,
           F.Rating
    FROM dbo.Film AS F
    WHERE F.Rating IS NOT NULL
      AND F.Rating >= @minRating
    ORDER BY F.Rating DESC, F.Title;
END
GO
