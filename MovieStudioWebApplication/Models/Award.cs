using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Award
    {
        [Key]
        [Display(Name = "ID Награды")]
        public int AwardID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Год")]
        public int Year { get; set; }
        [Display(Name = "Категория")]
        public string Category { get; set; }
        [Display(Name = "Имя победителя")]
        public string WinnerName { get; set; }
        [Display(Name = "Место проведения церемонии")]
        public string CeremonyLocation { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual AwardRecipient AwardRecipient { get; set; }
    }
}