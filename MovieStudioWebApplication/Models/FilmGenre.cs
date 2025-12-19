using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class FilmGenre
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Film")]
        public int FilmID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Genre")]
        public int GenreID { get; set; }

        public virtual Film Film { get; set; }
        public virtual Genre Genre { get; set; }
    }
}