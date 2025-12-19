using System.ComponentModel.DataAnnotations;

namespace MovieStudioWebApplication.Models
{
    public class FilmIndexViewModel
    {
        public int FilmID { get; set; }
        public string Title { get; set; }
        [Display(Name = "Режиссер")]
        public string DirectorFullName { get; set; }
        [Display(Name = "Студия")]
        public string StudioName { get; set; }
        [Display(Name = "Год выпуска")]
        public int ReleaseYear { get; set; }
        [Display(Name = "Продолжительность")]
        public int DurationMinutes { get; set; }
        [Display(Name = "Бюджет")]
        public decimal Budget { get; set; }
        [Display(Name = "Сборы")]
        public decimal BoxOffice { get; set; }
        [Display(Name = "Рейтинг")]
        public decimal Rating { get; set; }
    }
}
