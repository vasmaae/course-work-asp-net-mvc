using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class AwardRecipient
    {
        [Key, ForeignKey("Award")]
        public int AwardID { get; set; }

        public int? FilmID { get; set; }
        public int? ActorID { get; set; }
        public int? DirectorID { get; set; }

        public virtual Award Award { get; set; }
        [ForeignKey("FilmID")]
        public virtual Film Film { get; set; }
        [ForeignKey("ActorID")]
        public virtual Actor Actor { get; set; }
        [ForeignKey("DirectorID")]
        public virtual Director Director { get; set; }
    }
}