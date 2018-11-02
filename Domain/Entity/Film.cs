using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain.Entity
{
  public  class Film
    {

        [Key]
        public int FilmId { get; set; }
        [Required]
        public string FilmTitle { get; set; }
        [Required]
        public string FilmDescription { get; set; }
        [Required]
        public int FilmNote { get; set; }

        public virtual List<File> Images { get; set; }
        public virtual List<Category> Categories { get; set; }
        public virtual List<Acteur> Acteurs { get; set; }
        public virtual List<Projection> Projections { get; set; }
       

        public override string ToString()
        {
          

            return "";
        }
    }
}
