using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using Domain.Entity;
using Data.Infrastructure;
using System;
using System.Configuration;
using System.Data.SqlClient;
using Service.Interfaces;

namespace Service
{
    public class FilmService : Service<Film>, IFilmService
    {
       
       
        private static IDatabaseFactory dbf =  new DatabaseFactory();
        private static IUnitOfWork ut = new UnitOfWork(dbf);
        public FilmService()
           : base(ut)
        {
           
           

        }

      
        #region imp
        //verifier
        public IEnumerable<Film> GetFilmByActeur(int idActeur)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmActeur";
            var FilmIds = new List<int>();
            string req = "SELECT Film FROM " + tableName + " WHERE Acteur = " + idActeur;
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            SqlDataReader reader = CMD.ExecuteReader();
            while (reader.Read())
            {
                FilmIds.Add(int.Parse(reader["Film"].ToString()));
            }
            Conn.Close();
            reader.Close();
            return ut.getRepository<Film>().GetMany(f => FilmIds.Contains(f.FilmId));

        }
        //verifier
        public IEnumerable<Film> GetFilmByCategory(int idCategory)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmCategory";
            var FilmIds = new List<int>();
            string req = "SELECT Film FROM " + tableName + " WHERE Category = "+idCategory;
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            SqlDataReader reader = CMD.ExecuteReader();
            while (reader.Read())
            {
                FilmIds.Add (int.Parse(reader["Film"].ToString()));
            }
            Conn.Close();
            reader.Close();
            return ut.getRepository<Film>().GetMany(f =>FilmIds.Contains(f.FilmId));

        }
        //verifier
        public IEnumerable<Category> GetFilmCategories(int idFilm)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmCategory";
            var CatIds = new List<int>();
            string req = "SELECT Category FROM " + tableName + " WHERE Film = " + idFilm;
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            SqlDataReader reader = CMD.ExecuteReader();
            while (reader.Read())
            {
                CatIds.Add(int.Parse(reader["Category"].ToString()));
            }
            Conn.Close();
            reader.Close();
            return ut.getRepository<Category>().GetMany(c => CatIds.Contains(c.CategoryId));
        }
        //verifier
        public IEnumerable<Acteur> GetFilmActeurs(int idFilm)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmActeur";
            var ActIds = new List<int>();
            string req = "SELECT Acteur FROM " + tableName + " WHERE Film = " + idFilm;
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            SqlDataReader reader = CMD.ExecuteReader();
            while (reader.Read())
            {
                ActIds.Add(int.Parse(reader["Acteur"].ToString()));
            }
            Conn.Close();
            reader.Close();
            return ut.getRepository<Acteur>().GetMany(a => ActIds.Contains(a.ActeurId));

        }
        //verifier
        public IEnumerable<File> GetFilmImages(int idFilm)
        {
            return ut.getRepository<File>().GetMany(f => f.FilmId == idFilm);
        }
        //verifier
        public IEnumerable<Projection> GetFilmProjections(int idFilm)
        {
            return ut.getRepository<Projection>().GetMany(p => p.FilmId == idFilm);
        }


        //verifier
        public void DeleteFilmFromCategory(int idCategory, int idFilm)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmCategory";
            string req = "DELETE  FROM " + tableName + " WHERE Film = " + idFilm + " AND Category = " + idCategory;
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            CMD.ExecuteNonQuery();
            Conn.Close();
        }
        //verifier
        public void AddFilmToCategory(int idCategory, int idFilm)
        {
            var connString = ConfigurationManager.ConnectionStrings["CinemaConnectionString"].ConnectionString;
            var tableName = "FilmCategory";
            string req = "INSERT  INTO " + tableName + " ( Film , Category ) VALUES(" + idFilm + "," + idCategory + ")";
            SqlConnection Conn = getConnectionInstance(connString);
            SqlCommand CMD = new SqlCommand(req, Conn);
            CMD.ExecuteNonQuery();
            Conn.Close();
        }

        #endregion

    }

}
