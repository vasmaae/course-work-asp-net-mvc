using System.ComponentModel.DataAnnotations;

namespace MovieStudioWebApplication.Models
{
    public class GenreIndexViewModel
    {
        public int GenreID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Количество фильмов")]
        public int FilmCount { get; set; }
    }
}
