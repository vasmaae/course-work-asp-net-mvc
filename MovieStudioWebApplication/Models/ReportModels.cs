namespace MovieStudioWebApplication.Models
{
    public class GenreFilmCountReportItem
    {
        public int GenreID { get; set; }
        public string GenreName { get; set; }
        public int FilmCount { get; set; }
    }

    public class FilmWithDirectorReportItem
    {
        public int FilmID { get; set; }
        public string Title { get; set; }
        public string DirectorName { get; set; }
        public decimal? Rating { get; set; }
    }

    public class TopProfitFilmReportItem
    {
        public int FilmID { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public decimal Profit { get; set; }
    }

    public class ReportsViewModel
    {
        public decimal MinProfit { get; set; }
        public int? Year { get; set; }
        public GenreFilmCountReportItem[] GenreCounts { get; set; }
        public FilmWithDirectorReportItem[] FilmsWithDirectors { get; set; }
        public TopProfitFilmReportItem[] TopProfitFilms { get; set; }
    }
}
