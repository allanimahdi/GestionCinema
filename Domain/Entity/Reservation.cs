using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
   public class Reservation
    {

        [Key]
        public int ReservationId { get; set; }
        public int ProjectionId { get; set; }
        public virtual Projection Projection { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
