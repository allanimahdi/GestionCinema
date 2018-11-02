using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
     public class ProjectionConfiguration : EntityTypeConfiguration<Projection>
    {
        public ProjectionConfiguration()
        {
            //One To Many
            HasRequired(p => p.Salle)
                .WithMany(s => s.Projections)
                .HasForeignKey(p => p.SalleId)
                .WillCascadeOnDelete(true);

            HasRequired(p => p.Film)
                .WithMany(f => f.Projections)
                .HasForeignKey(p => p.FilmId)
                .WillCascadeOnDelete(true);
        }
    }
}
