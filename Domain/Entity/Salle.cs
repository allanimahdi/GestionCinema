using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
   public class Salle
    {
        [Key]
        public int SalleId { get; set; }
        public string NomSalle { get; set; }

        public Adresse AdresseSalle { get; set; }

        public virtual List<Projection> Projections { get; set; }
    }
}
