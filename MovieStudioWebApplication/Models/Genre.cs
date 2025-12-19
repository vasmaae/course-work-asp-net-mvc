using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Genre
    {
        [Key]
        [Display(Name = "ID Жанра")]
        public int GenreID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}