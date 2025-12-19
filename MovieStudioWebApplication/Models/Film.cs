using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Film
    {
        public Film()
        {
            this.FilmGenres = new HashSet<FilmGenre>();
            this.FilmActors = new HashSet<FilmActor>();
            this.AwardRecipients = new HashSet<AwardRecipient>();
        }

        [Key]
        [Display(Name = "ID Фильма")]
        public int FilmID { get; set; }

        [ForeignKey("Studio")]
        [Display(Name = "ID Студии")]
        public int StudioID { get; set; }

        [ForeignKey("Director")]
        [Display(Name = "ID Режиссера")]
        public int DirectorID { get; set; }

        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Год выпуска")]
        public int ReleaseYear { get; set; }
        [Display(Name = "Продолжительность (минут)")]
        public int DurationMinutes { get; set; }
        [Display(Name = "Бюджет")]
        public decimal Budget { get; set; }
        [Display(Name = "Кассовые сборы")]
        public decimal? BoxOffice { get; set; }
        [Display(Name = "Рейтинг")]
        public decimal? Rating { get; set; }
        [Display(Name = "Синопсис")]
        public string Synopsis { get; set; }

        public virtual Studio Studio { get; set; }
        public virtual Director Director { get; set; }
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
        public virtual ICollection<FilmActor> FilmActors { get; set; }
        public virtual ICollection<AwardRecipient> AwardRecipients { get; set; }
    }
}