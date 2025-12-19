using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class Employee
    {
        [Key]
        [Display(Name = "ID Сотрудника")]
        public int EmployeeID { get; set; }

        [ForeignKey("Department")]
        [Display(Name = "ID Отдела")]
        public int DepartmentID { get; set; }

        [ForeignKey("Director")]
        [Display(Name = "ID Помощника режиссера")]
        public int? DirectorAssistantID { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Display(Name = "Дата найма")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? HireDate { get; set; }
        [Display(Name = "Зарплата")]
        public decimal Salary { get; set; }
        [Display(Name = "Контактный телефон")]
        public string ContactPhone { get; set; }
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        public virtual Department Department { get; set; }
        public virtual Director Director { get; set; }
    }
}