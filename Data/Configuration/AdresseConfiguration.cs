using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
   public class AdresseConfiguration : EntityTypeConfiguration<Adresse>
    {
        public AdresseConfiguration()
        {
           /* Property(p => p.Ville).IsRequired();
            Property(p => p.Rue).HasMaxLength(50)
                .IsOptional();
            Property(p => p.FormattedAdress)
               .IsOptional();*/
        }
    }
}
