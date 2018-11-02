using Domain.Entity;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    /// <summary>
    ///     Add any custom business logic (methods) here
    /// </summary>
    public interface IFilmService : IService<Film>
    {
        IEnumerable<Film> GetFilmByCategory(int idCategory);
        IEnumerable<Film> GetFilmByActeur(int idActeur);


        IEnumerable<File> GetFilmImages(int idFilm);
        IEnumerable<Acteur> GetFilmActeurs(int idFilm);
        IEnumerable<Category> GetFilmCategories(int idFilm);
        IEnumerable<Projection> GetFilmProjections(int idFilm);

 

        void DeleteFilmFromCategory(int idCategory,int idFilm);
        void AddFilmToCategory(int idCategory, int idFilm);
    }

}
