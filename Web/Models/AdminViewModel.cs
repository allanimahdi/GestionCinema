using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class AdminViewModel
    {

        public List<Film> Films { get; set; }

        public List<Category> Categories { get; set; }

        public List<Acteur> Acteurs { get; set; }

        public List<Projection> Projections { get; set; }

        public List<Salle> Salles { get; set; }

        public List<File> Files { get; set; }

        public List<Event> Events { get; set; }

        public List<Participation> Participations { get; set; }

        public List<User> Users { get; set; }

        public List<Reclamation> Reclamations { get; set; }
        public List<Role> Roles { get; set; }

    }


}