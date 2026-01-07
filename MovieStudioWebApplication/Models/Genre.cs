using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieStudioWebApplication.Models
{
    public class Genre
    {
        public Genre()
        {
            this.Films = new HashSet<Film>();
        }
        [Key]
        [Display(Name = "ID Жанра")]
        public int GenreID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<Film> Films { get; set; }
    }
}
