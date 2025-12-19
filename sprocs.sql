-- =============================================
-- Author:      Gemini
-- Create date: 2025-12-19
-- Description: Gets the full name of a director.
-- =============================================
CREATE FUNCTION dbo.GetDirectorFullName
(
    @directorId INT
)
RETURNS NVARCHAR(101)
AS
BEGIN
    DECLARE @fullName NVARCHAR(101);

    SELECT @fullName = FirstName + ' ' + LastName
    FROM dbo.Directors
    WHERE DirectorID = @directorId;

    RETURN @fullName;
END
GO

-- =============================================
-- Author:      Gemini
-- Create date: 2025-12-19
-- Description: Gets the number of films for a given genre.
-- =============================================
CREATE FUNCTION dbo.GetFilmCountForGenre
(
    @genreId INT
)
RETURNS INT
AS
BEGIN
    DECLARE @filmCount INT;

    SELECT @filmCount = COUNT(FG.FilmID)
    FROM dbo.FilmGenres AS FG
    WHERE FG.GenreID = @genreId;

    RETURN @filmCount;
END
GO

-- =============================================
-- Author:      Gemini
-- Create date: 2025-12-19
-- Description: Gets all actors for a specific film.
-- =============================================
CREATE PROCEDURE dbo.GetActorsByFilm
    @filmId INT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    -- Select statements for procedure here
    SELECT A.ActorID, A.FirstName, A.LastName, A.DateOfBirth
    FROM dbo.Actors AS A
    INNER JOIN dbo.FilmActors AS FA ON A.ActorID = FA.ActorID
    WHERE FA.FilmID = @filmId
    ORDER BY A.LastName, A.FirstName;
END
GO
