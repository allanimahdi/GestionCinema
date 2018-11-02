using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
  public  class Event
    {
        [Key]
        public int EventId { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Description { get; set; }
        public string Titre { get; set; }
        public Adresse Adresse { get; set; }
        public int Capacite { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
    }
}
