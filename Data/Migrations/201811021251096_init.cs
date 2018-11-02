namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Acteurs",
                c => new
                    {
                        ActeurId = c.Int(nullable: false, identity: true),
                        NomActeur = c.String(),
                        PrenomActeur = c.String(),
                        NoteActeur = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActeurId);
            
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        FilmId = c.Int(nullable: false, identity: true),
                        FilmTitle = c.String(nullable: false),
                        FilmDescription = c.String(nullable: false),
                        FilmNote = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilmId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 255),
                        ContentType = c.String(maxLength: 100),
                        Content = c.Binary(),
                        FileType = c.Int(nullable: false),
                        FilmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true)
                .Index(t => t.FilmId);
            
            CreateTable(
                "dbo.Projections",
                c => new
                    {
                        ProjectionId = c.Int(nullable: false, identity: true),
                        DateProjection = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SalleId = c.Int(nullable: false),
                        FilmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectionId)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true)
                .ForeignKey("dbo.Salles", t => t.SalleId, cascadeDelete: true)
                .Index(t => t.SalleId)
                .Index(t => t.FilmId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        ProjectionId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReservationId)
                .ForeignKey("dbo.Projections", t => t.ProjectionId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Participations",
                c => new
                    {
                        ParticipationId = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ParticipationId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.EventId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        DateDebut = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateFin = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Description = c.String(),
                        Titre = c.String(),
                        Adresse_Ville = c.String(),
                        Adresse_Rue = c.String(),
                        Adresse_FormattedAdress = c.String(),
                        Capacite = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Salles",
                c => new
                    {
                        SalleId = c.Int(nullable: false, identity: true),
                        NomSalle = c.String(),
                        AdresseSalle_Ville = c.String(),
                        AdresseSalle_Rue = c.String(),
                        AdresseSalle_FormattedAdress = c.String(),
                    })
                .PrimaryKey(t => t.SalleId);
            
            CreateTable(
                "dbo.Reclamations",
                c => new
                    {
                        ReclamationId = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        Sujet = c.String(nullable: false, maxLength: 200),
                        état = c.String(),
                    })
                .PrimaryKey(t => t.ReclamationId);
            
            CreateTable(
                "dbo.FilmActeur",
                c => new
                    {
                        Film = c.Int(nullable: false),
                        Acteur = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Film, t.Acteur })
                .ForeignKey("dbo.Films", t => t.Film, cascadeDelete: true)
                .ForeignKey("dbo.Acteurs", t => t.Acteur, cascadeDelete: true)
                .Index(t => t.Film)
                .Index(t => t.Acteur);
            
            CreateTable(
                "dbo.FilmCategory",
                c => new
                    {
                        Film = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Film, t.Category })
                .ForeignKey("dbo.Films", t => t.Film, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category, cascadeDelete: true)
                .Index(t => t.Film)
                .Index(t => t.Category);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projections", "SalleId", "dbo.Salles");
            DropForeignKey("dbo.Reservations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Permission", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permission", "UserId", "dbo.Users");
            DropForeignKey("dbo.Participations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Participations", "EventId", "dbo.Events");
            DropForeignKey("dbo.Reservations", "ProjectionId", "dbo.Projections");
            DropForeignKey("dbo.Projections", "FilmId", "dbo.Films");
            DropForeignKey("dbo.Files", "FilmId", "dbo.Films");
            DropForeignKey("dbo.FilmCategory", "Category", "dbo.Categories");
            DropForeignKey("dbo.FilmCategory", "Film", "dbo.Films");
            DropForeignKey("dbo.FilmActeur", "Acteur", "dbo.Acteurs");
            DropForeignKey("dbo.FilmActeur", "Film", "dbo.Films");
            DropIndex("dbo.Permission", new[] { "RoleId" });
            DropIndex("dbo.Permission", new[] { "UserId" });
            DropIndex("dbo.FilmCategory", new[] { "Category" });
            DropIndex("dbo.FilmCategory", new[] { "Film" });
            DropIndex("dbo.FilmActeur", new[] { "Acteur" });
            DropIndex("dbo.FilmActeur", new[] { "Film" });
            DropIndex("dbo.Participations", new[] { "UserId" });
            DropIndex("dbo.Participations", new[] { "EventId" });
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Reservations", new[] { "ProjectionId" });
            DropIndex("dbo.Projections", new[] { "FilmId" });
            DropIndex("dbo.Projections", new[] { "SalleId" });
            DropIndex("dbo.Files", new[] { "FilmId" });
            DropTable("dbo.Permission");
            DropTable("dbo.FilmCategory");
            DropTable("dbo.FilmActeur");
            DropTable("dbo.Reclamations");
            DropTable("dbo.Salles");
            DropTable("dbo.Roles");
            DropTable("dbo.Events");
            DropTable("dbo.Participations");
            DropTable("dbo.Users");
            DropTable("dbo.Reservations");
            DropTable("dbo.Projections");
            DropTable("dbo.Files");
            DropTable("dbo.Categories");
            DropTable("dbo.Films");
            DropTable("dbo.Acteurs");
        }
    }
}
