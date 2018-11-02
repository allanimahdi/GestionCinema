using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
  public  class Reclamation
    {
        [Key]
        public int ReclamationId { get; set; }


        [Required]
        [Display(Name = "Votre nom ")]
        [StringLength(50)]
        public string Nom { get; set; }

        [Required, EmailAddress]
        [StringLength(50)]
        public string
            Email
        { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(200)]
        public string Sujet { get; set; }
        public string état { get; set; }
    }
}
