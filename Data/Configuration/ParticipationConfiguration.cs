using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
   public class ParticipationConfiguration : EntityTypeConfiguration<Participation>
    {
        public ParticipationConfiguration()
        {
            //One To Many
            HasRequired(p => p.Event)
                .WithMany(e => e.Participations)
                .HasForeignKey(p => p.EventId)
                .WillCascadeOnDelete(true);

            HasRequired(p => p.User)
                .WithMany(u => u.Participations)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
        }

    }
}
