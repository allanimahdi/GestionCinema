using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class HomeViewModel
    {

        public List<Film> Films { get; set; }

        public List<Category> Categories { get; set; }
    }
}