using Data.Infrastructure;
using Domain.Entity;
using Service.Interfaces;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
   public class ReclamationService : Service<Reclamation>, IReclamationService
    {
        private static IDatabaseFactory dbf = new DatabaseFactory();
        private static IUnitOfWork ut = new UnitOfWork(dbf);
        public ReclamationService()
           : base(ut)
        {



        }
    }
}
