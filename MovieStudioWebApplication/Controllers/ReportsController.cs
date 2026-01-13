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

        public ActionResult Index(decimal? minProfit, int? year)
        {
            var minProfitValue = minProfit ?? 0m;

            var genreCounts = db.Database.SqlQuery<GenreFilmCountReportItem>(
                "SELECT G.GenreID, G.Name AS GenreName, dbo.GetFilmCountForGenre(G.GenreID) AS FilmCount " +
                "FROM Genre AS G ORDER BY G.Name").ToArray();

            var filmsWithDirectors = db.Database.SqlQuery<FilmWithDirectorReportItem>(
                "SELECT F.FilmID, F.Title, dbo.GetDirectorFullName(F.DirectorID) AS DirectorName, F.Rating " +
                "FROM Film AS F ORDER BY F.Title").ToArray();

            var profitParam = new SqlParameter("@minProfit", minProfitValue);
            var yearParam = new SqlParameter("@year", (object)year ?? DBNull.Value);
            var topProfitFilms = db.Database.SqlQuery<TopProfitFilmReportItem>(
                "EXEC dbo.GetTopFilmsByProfit @minProfit, @year", profitParam, yearParam).ToArray();

            var viewModel = new ReportsViewModel
            {
                MinProfit = minProfitValue,
                Year = year,
                GenreCounts = genreCounts,
                FilmsWithDirectors = filmsWithDirectors,
                TopProfitFilms = topProfitFilms
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
