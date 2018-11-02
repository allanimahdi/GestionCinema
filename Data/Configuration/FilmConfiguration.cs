using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configuration
{
  public  class FilmConfiguration : EntityTypeConfiguration<Film>
    {
        public FilmConfiguration()
        {
            

            //Many to Many configuration
            HasMany(p => p.Acteurs)
            .WithMany(v => v.Films)
            .Map(m =>
            {
                m.ToTable("FilmActeur");   
                m.MapLeftKey("Film");
                m.MapRightKey("Acteur");
            });

            HasMany(p => p.Categories)
           .WithMany(c => c.Films)
           .Map(m =>
           {
               m.ToTable("FilmCategory");
               m.MapLeftKey("Film");
               m.MapRightKey("Category");
           });

        }
    }
}
