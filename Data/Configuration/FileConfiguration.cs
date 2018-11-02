using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
   public class FileConfiguration : EntityTypeConfiguration<File>
    {
        public FileConfiguration()
        {
            //One To Many
            HasRequired(p => p.Film)
                .WithMany(s => s.Images)
                .HasForeignKey(p => p.FilmId)
                .WillCascadeOnDelete(true);
        }
    }
}
