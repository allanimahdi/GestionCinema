
using Domain.Entity;
using Service;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Web.Security;

namespace Web.Controllers
{
   
     [CustomAuthorize(Roles= "User")]
     [Authorize]
    public class UserController : BaseController
    {
        IParticipationService participationService = new ParticipationService();
        IEventService eventService = new EventService();
        IUserService userService = new UserService();
        //
        // GET: /User/
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home",null);
        }
      
    }
}