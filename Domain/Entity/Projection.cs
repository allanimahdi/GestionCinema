using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
  public  class Projection
    {
        [Key]
        public int ProjectionId { get; set; }
        public DateTime DateProjection { get; set; }

        public int SalleId { get; set; }
        public virtual Salle Salle { get; set; }
        public int FilmId { get; set; }
        public virtual Film Film { get; set; }


        public virtual ICollection<Reservation> Reservations { get; set; }

    }
}
