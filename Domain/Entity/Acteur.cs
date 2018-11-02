using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
  public  class Acteur
    {
        [Key]
        public int ActeurId { get; set; }
        public string NomActeur { get; set; }
        public string PrenomActeur { get; set; }
        public int NoteActeur { get; set; }
        public virtual List<Film> Films { get; set; }
    }
}
