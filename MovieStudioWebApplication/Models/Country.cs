using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Country
    {
        [Key]
        [Display(Name = "ID Страны")]
        public int CountryID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }

        public virtual ICollection<Director> Directors { get; set; }
        public virtual ICollection<Actor> Actors { get; set; }
    }
}