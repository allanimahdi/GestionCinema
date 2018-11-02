using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
  public  class Adresse
    {
        [Display(Name = "Ville")]
        
        public string Ville { get; set; }
        [Display(Name = "Rue")]
        public string Rue { get; set; }
        [Display(Name = "Adresse Complette")]
        public string FormattedAdress { get; set; }
    }
}
