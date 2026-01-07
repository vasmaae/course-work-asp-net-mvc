using MovieStudioWebApplication.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MovieStudioWebApplication.Controllers
{
    public class ReportsController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        public ActionResult Index(decimal? minRating)
        {
            var rating = minRating ?? 7.0m;

            var genreCounts = db.Database.SqlQuery<GenreFilmCountReportItem>(
                "SELECT G.GenreID, G.Name AS GenreName, dbo.GetFilmCountForGenre(G.GenreID) AS FilmCount " +
                "FROM Genre AS G ORDER BY G.Name").ToArray();

            var filmsWithDirectors = db.Database.SqlQuery<FilmWithDirectorReportItem>(
                "SELECT F.FilmID, F.Title, dbo.GetDirectorFullName(F.DirectorID) AS DirectorName, F.Rating " +
                "FROM Film AS F ORDER BY F.Title").ToArray();

            var ratingParam = new SqlParameter("@minRating", rating);
            var topRatedFilms = db.Database.SqlQuery<TopRatedFilmReportItem>(
                "EXEC dbo.GetTopFilmsByRating @minRating", ratingParam).ToArray();

            var viewModel = new ReportsViewModel
            {
                MinRating = rating,
                GenreCounts = genreCounts,
                FilmsWithDirectors = filmsWithDirectors,
                TopRatedFilms = topRatedFilms
            };

            return View(viewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
