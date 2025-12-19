using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieStudioWebApplication.Models
{
    public class FilmViewModel
    {
        public Film Film { get; set; }
        public List<AssignedGenreData> Genres { get; set; }
        public List<AssignedActorData> Actors { get; set; }
    }

    public class AssignedGenreData
    {
        public int GenreID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }

    public class AssignedActorData
    {
        public int ActorID { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}