using Data.Configuration;
using Data.Convention;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data
{
   public class CinemaContext:DbContext
    {
        public CinemaContext():base("Name=CinemaConnectionString")
        {

        }
        public DbSet<Film> Films { get; set; }
        public DbSet<Salle> Salles { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Acteur> Acteurs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        
            modelBuilder.Configurations.Add(new FilmConfiguration());
            modelBuilder.Configurations.Add(new ProjectionConfiguration());
            modelBuilder.Configurations.Add(new ParticipationConfiguration());
            modelBuilder.Configurations.Add(new AdresseConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new FileConfiguration());
            modelBuilder.Configurations.Add(new ReservationConfiguration());
            modelBuilder.Conventions.Add(new DateTime2Convention());
        }
    }
  
}
