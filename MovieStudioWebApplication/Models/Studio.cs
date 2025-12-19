using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Studio
    {
        [Key]
        [Display(Name = "ID Студии")]
        public int StudioID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Расположение")]
        public string Location { get; set; }
        [Display(Name = "Год основания")]
        public int FoundedYear { get; set; }
        [Display(Name = "Количество сотрудников")]
        public int? EmployeeCount { get; set; }
        [Display(Name = "Бюджет")]
        public decimal Budget { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Film> Films { get; set; }
    }
}