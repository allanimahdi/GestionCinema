using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
  public  class ReservationConfiguration : EntityTypeConfiguration<Reservation>
    {
        public ReservationConfiguration()
        {
            //One To Many
            HasRequired(p => p.Projection)
                .WithMany(e => e.Reservations)
                .HasForeignKey(p => p.ProjectionId)
                .WillCascadeOnDelete(true);

            HasRequired(p => p.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
        }

    }
}
