using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
   public class Participation
    {
        public int ParticipationId { get; set; }
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
