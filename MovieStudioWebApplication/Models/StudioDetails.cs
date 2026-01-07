using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStudioWebApplication.Models
{
    public class StudioDetails
    {
        [Key, ForeignKey("Studio")]
        [Display(Name = "ID Студии")]
        public int StudioID { get; set; }

        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Веб-сайт")]
        public string Website { get; set; }

        public virtual Studio Studio { get; set; }
    }
}
