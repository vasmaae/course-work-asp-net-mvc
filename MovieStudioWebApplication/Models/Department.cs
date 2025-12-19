using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStudioWebApplication.Models
{
    public class Department
    {
        [Key]
        [Display(Name = "ID Отдела")]
        public int DepartmentID { get; set; }
        [ForeignKey("Studio")]
        [Display(Name = "ID Студии")]
        public int StudioID { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Имя руководителя")]
        public string HeadName { get; set; }
        [Display(Name = "Количество сотрудников")]
        public int? EmployeeCount { get; set; }
        [Display(Name = "Бюджет")]
        public decimal Budget { get; set; }
        [Display(Name = "Обязанности")]
        public string Responsibilities { get; set; }

        public virtual Studio Studio { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}