using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
 public   class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual List<Film> Films { get; set; }


        public override int GetHashCode()
        {
            return CategoryId * CategoryName.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj!=null)
            {
                Category c = obj as Category;
                if (CategoryName.Equals(c.CategoryName))
                {
                    return true;
                }
            }
          
            return false;
        }
    }
}
