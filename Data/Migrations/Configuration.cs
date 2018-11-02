namespace Data.Migrations
{
    using Domain.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.CinemaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.CinemaContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Categories.AddOrUpdate(
              p => p.CategoryName,
                new Category { CategoryName = "Action" },
                new Category { CategoryName = "Comédie" },
                new Category { CategoryName = "Fantastique" },
                new Category { CategoryName = "Horreur" },
                new Category { CategoryName = "Aventure" }
            );
            context.Salles.AddOrUpdate(
           s=>s.NomSalle,
           new Salle { NomSalle="S1",AdresseSalle=new Adresse {Rue="haj kasem",Ville="ariana",FormattedAdress="35 haj kacem ariana" } },
           new Salle { NomSalle = "S2", AdresseSalle = new Adresse { Rue = "haj Ali", Ville = "ben arous", FormattedAdress = "706 haj ali ben arous" } },
           new Salle { NomSalle = "S3", AdresseSalle = new Adresse { Rue = "hbib bourgiba", Ville = "tunis", FormattedAdress = "11 hbib bourgiba  tunis" } }
         );

            Role roleAdmin = new Role { RoleName = "Admin", Description = "administrateur du site" ,Users=new List<User>()};
            Role roleUser = new Role { RoleName = "User", Description = "utilisateur du site", Users = new List<User>() };
            User admin = new User() {Username="admin",CreateDate=DateTime.Now,Email="admin@admin.com",IsActive=true,Password="123456",FirstName="admin",LastName="admin",Roles=new List<Role>() };
            User user = new User() { Username = "user", CreateDate = DateTime.Now, Email = "user@user.com", IsActive = true, Password = "123456", FirstName = "user", LastName = "user", Roles = new List<Role>() };

            roleAdmin.Users.Add(admin);
            roleUser.Users.Add(user);
            admin.Roles.Add(roleAdmin);
            user.Roles.Add(roleUser);

            context.Roles.AddOrUpdate(
                r => r.RoleName,
               roleAdmin,
               roleUser                
                );
            context.Users.AddOrUpdate(
                u => u.Username,
               admin,
               user
                );

            
        }
    }
}
