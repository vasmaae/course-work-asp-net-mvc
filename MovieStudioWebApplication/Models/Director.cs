using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Director
    {
        [Key]
        [Display(Name = "ID Режиссера")]
        public int DirectorID { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Дата рождения")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Национальность")]
        public string Nationality { get; set; }
        [Display(Name = "Лет опыта")]
        public int? ExperienceYears { get; set; }
        [Display(Name = "Биография")]
        public string Biography { get; set; }

        [ForeignKey("Country")]
        [Display(Name = "Страна")]
        public int? CountryID { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<Film> Films { get; set; }
        public virtual ICollection<AwardRecipient> AwardRecipients { get; set; }
    }
}