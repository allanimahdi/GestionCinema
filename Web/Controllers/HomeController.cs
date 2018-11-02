using Data;
using Domain.Entity;
using Service;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

using System.Web.Mvc;
using Web.Models;
using Web.DAL;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        IFilmService filmService = new FilmService();
        ICategoryService categoryService = new CategoryService();
        IEventService eventService = new EventService();
        public static string msg;
        public ActionResult Index()
        {
            if (msg != null)
            {
                if (msg.Equals("login-e"))
                {
                    ViewBag.error = "Incorrect username and / or password"; 
                }

                if (msg.Equals("regiter-s"))
                {
                    ViewBag.success = "Login now to active your account";
                }
                if (msg.Equals("res-e"))
                {
                    ViewBag.error = "login please";
                }
                if (msg.Equals("place-e"))
                {
                    ViewBag.error = "pas de place sorry";
                }
                msg = null;
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Film(int?cat)
        {
            ViewBag.Message = "Welcom  To Film Section.";
            List<Category> Categories = categoryService.GetMany().ToList();
            List<Film> Films = new List<Film>(); 
            if (cat == null)
            {
                Films= filmService.GetMany().ToList();

            } else {
                Films = filmService.GetFilmByCategory(cat.Value).ToList();
        
            }
            
            foreach (var item in Films)
            {
                if (item.Categories == null)
                {
                    item.Categories = filmService.GetFilmCategories(item.FilmId).ToList();
                }
                if (item.Images == null)
                {
                    item.Images = filmService.GetFilmImages(item.FilmId).ToList();
                }
            }
                HomeViewModel homeViewModel = new HomeViewModel { Films = Films, Categories = Categories };

            return View(homeViewModel);
        }
        public ActionResult Event() {

            ViewBag.Message = "Welcom  To Event Section.";

            List<Event> Events = eventService.GetMany().ToList();

            return View(Events);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Reclamation()
        {
            string name = Request["name"];
            string sujet = Request["sujet"];
            string email = Request["email"];
            Reclamation c = new Reclamation() { Sujet=sujet,Nom=name,Email=email,état="non lu"};
            IReclamationService csv = new ReclamationService();
            csv.Add(c);
            csv.Commit();

           return RedirectToAction("Contact");
           
        }

        public ActionResult Salle()
        {
            ISalleService scv = new SalleService();
            IProjectionService pcv = new ProjectionService();
            List<Salle> salles = scv.GetMany().ToList();
            foreach (var item in salles)
            {
                item.Projections = pcv.GetMany(p => p.SalleId == item.SalleId).ToList();
            }
            return View(salles);

        }


        public ActionResult TopFilm()
        {
            
            List<Film> films = filmService.GetMany(f=>f.FilmNote>=4).ToList();
            IProjectionService pcv = new ProjectionService();
            foreach (var item in films)
            {
                item.Projections = pcv.GetMany(p => p.FilmId == item.FilmId).ToList();
                item.Categories = filmService.GetFilmCategories(item.FilmId).ToList();
            }




            return PartialView(films);

        }




        #region Statestique


        public ActionResult StatFilm()
        {

            return View("Statestique/StatFilm");
        }

        public ActionResult StatFilmParNote()
        {

            List<Film> films = filmService.GetMany().ToList();
            List<dynamic> models = new List<dynamic>();
            int index = 0;
            foreach (var item in films)
            {
                if (index <= 5)
                {
                    dynamic x = new
                    {
                        label = index + " Stars",
                        value = films.Where(f => f.FilmNote == index).Count()
                    };

                    models.Add(x);
                    index++;
                }
                else {
                    break;
                }
              
            }

            return View("Statestique/StatFilmParNote", models);
        }
        public ActionResult StatFilmParProj()
        {

            List<Film> fls = filmService.GetMany().ToList();
            List<dynamic> models = new List<dynamic>();
            foreach (var item in fls)
            {
                dynamic x = new
                {
                    label = item.FilmTitle,
                    value = item.Projections.Count()
                };

                models.Add(x);
            }

            return View("Statestique/StatFilmParProj", models);
        }

        public ActionResult historiqueEv( int?id)
        {

            WebContext ww = new WebContext();
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Participation> part = ww.Participations.Where(p => p.UserId == id.Value).ToList();
            
            return View(part);
        }

        public ActionResult historiquePro(int? id)
        {

            WebContext ww = new WebContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Reservation> part = ww.Reservations.Where(p => p.UserId == id.Value).ToList();
            return View(part);
        }

        #endregion

        public ActionResult Reservation(int? id)
        {
            if (User!= null)
            {


                WebContext ww = new WebContext();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Event @event = ww.Events.Find(id.Value);
                if (@event == null)
                {
                    return HttpNotFound();
                }
                User user = ww.Users.Find(User.UserId);
                if (@event.Capacite <= ww.Participations.Where(p => p.EventId == @event.EventId).Count())
                {
                    msg = "place-e";

                }
                else
                {
                    Participation p = new Participation() { EventId = @event.EventId, UserId = user.UserId };

                    @event.Capacite -= 1;
                    ww.Entry(@event).State = System.Data.Entity.EntityState.Modified;
                    ww.SaveChanges();
                    ww.Participations.Add(p);
                    ww.SaveChanges();
                    ViewBag.success = "Reservation of 1 ticket for " + user.Email + " to Event " + @event.Titre;
                }
            }
            else {
                msg = "res-e";
            }
            return RedirectToAction("Index");
        }
        public ActionResult ReservationProj(int? id)
        {
            if (User!=null)
            {

            
            WebContext ww = new WebContext();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projection @event = ww.Projections.Find(id.Value);
            if (@event == null)
            {
                return HttpNotFound();
            }
            User user = ww.Users.Find(User.UserId);
            
           
                Reservation p = new Reservation() { ProjectionId = @event.ProjectionId, UserId = user.UserId };

             
                ww.Reservations.Add(p);
                ww.SaveChanges();
                ViewBag.success = "Reservation of 1 ticket for " + user.Email + " to Projection of " + @event.Film.FilmTitle;

            }
            else
            {
                msg = "res-e";
            }
            return RedirectToAction("Index");
        }


        public ActionResult DeleteReservationProjection(int? id)
        {
            WebContext db = new WebContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }
        [HttpPost]
        public ActionResult DeleteReservationProjection(int id)
        {
            WebContext db = new WebContext();
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}