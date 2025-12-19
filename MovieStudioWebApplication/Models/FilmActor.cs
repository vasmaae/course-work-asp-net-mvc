using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class FilmActor
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Film")]
        public int FilmID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Actor")]
        public int ActorID { get; set; }

        public string Role { get; set; }

        public virtual Film Film { get; set; }
        public virtual Actor Actor { get; set; }
    }
}