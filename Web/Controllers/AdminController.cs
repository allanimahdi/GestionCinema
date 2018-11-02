using Data;
using Data.Infrastructure;
using Domain.Entity;
using Service;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.DAL;
using Web.Models;
using Web.Security;

namespace Web.Controllers
{
    [CustomAuthorize(Roles = "Admin")]
    [Authorize]
    public class AdminController : BaseController
    {



        AdminViewModel viewModel = new AdminViewModel();
        IFilmService filmService = new FilmService();
        IFileService fileService = new FileService();
        ICategoryService categorySevice = new CategoryService();
        IActeurService acteurSevice = new ActeurService();
        ISalleService salleSevice = new SalleService();
        IProjectionService projectionSevice = new ProjectionService();
        IEventService eventService = new EventService();
        IParticipationService participationService = new ParticipationService();
        IUserService userService = new UserService();
        IRoleService roleService = new RoleService();
        IReclamationService reclamationService = new ReclamationService();
        // GET: Admin

        public ActionResult Index()
        {
            viewModel.Films = filmService.GetMany().ToList();
            viewModel.Files = fileService.GetMany().ToList();
            viewModel.Categories = categorySevice.GetMany().ToList();
            viewModel.Acteurs = acteurSevice.GetMany().ToList();
            viewModel.Projections = projectionSevice.GetMany().ToList();
            viewModel.Salles = salleSevice.GetMany().ToList();
            viewModel.Events = eventService.GetMany().ToList();
            viewModel.Participations = participationService.GetMany().ToList();
            viewModel.Users = userService.GetMany().ToList();
            viewModel.Reclamations = reclamationService.GetMany().ToList();
            viewModel.Roles = ww.Roles.ToList();

            return View(viewModel);
        }
        #region Film
        // GET: FilmViewModels/Details/5
        [AllowAnonymous]
        public ActionResult DetailsFilm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Film film = filmService.GetById(id.Value);
            if (film == null)
            {
                return HttpNotFound();
            }
            film.Images = filmService.GetFilmImages(id.Value).ToList();
            film.Categories = filmService.GetFilmCategories(id.Value).ToList();
            return View("Film/Details",film);
        }

        // GET: FilmViewModels/Create
        public ActionResult CreateFilm()
        {
            return View("Film/Create");
        }

        // POST: Film/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFilm([Bind(Include = "FilmId,FilmTitle,FilmDescription,FilmNote")] Film filmViewModel, HttpPostedFileBase upload)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var image = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            image.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        filmViewModel.Images = new List<Domain.Entity.File> {image};
                    }
                    WebContext ww = new WebContext();
                    ww.Films.Add(filmViewModel);
                    ww.SaveChanges();
                    //filmService.Add(filmViewModel);
                    //filmService.Commit();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException )
            {
                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View("Film/Create",filmViewModel);
        }

        // GET: FilmViewModels/Edit/5
        public ActionResult EditFilm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Film film = filmService.GetById(id.Value);
            if (film == null)
            {
                return HttpNotFound();
            }
            film.Images = filmService.GetFilmImages(id.Value).ToList();
            film.Categories = filmService.GetFilmCategories(id.Value).ToList();
            ViewBag.CategorySelect = new SelectList(categorySevice.GetMany().ToList().Except(film.Categories), "CategoryId", "CategoryName");
            return View("Film/Edit",film);
        }

        // POST: FilmViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFilm([Bind(Include = "FilmId,FilmTitle,FilmDescription,FilmNote")] Film filmViewModel, HttpPostedFileBase upload)
        {
           
            if (filmViewModel.FilmId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = fileService.Get(fi => fi.FileType == FileType.Avatar && fi.FilmId == filmViewModel.FilmId);
                    avatar.FileName = System.IO.Path.GetFileName(upload.FileName);
                    avatar.FileType = FileType.Avatar;
                    avatar.ContentType = upload.ContentType;          
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatar.Content = reader.ReadBytes(upload.ContentLength);
                    }

                    fileService.Update(avatar);
                    fileService.Commit();
                }

                Film f = filmService.GetById(filmViewModel.FilmId);
                f.FilmNote = filmViewModel.FilmNote;
                f.FilmTitle = filmViewModel.FilmTitle;
                f.FilmDescription = filmViewModel.FilmDescription;

                filmService.Update(f);
                filmService.Commit();
                return RedirectToAction("Index");
            }
            return View("Film/Edit",filmViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFilmFromCategory()
        {
            var FilmId = int.Parse(Request["FilmId"]);
            var CategoryId = int.Parse(Request["CategoryId"]);
            var RedirectUrl = Request["RedirectUrl"];

            filmService.DeleteFilmFromCategory(CategoryId,FilmId);
            return Redirect(RedirectUrl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFilmToCategory()
        {
            var FilmId = int.Parse(Request["FilmId"]);
            var CategoryId = int.Parse(Request["CategorySelect"]);
            var RedirectUrl = Request["RedirectUrl"];

            filmService.AddFilmToCategory(CategoryId,FilmId);
            return Redirect(RedirectUrl);
        }
        // POST: FilmViewModels/Delete/5
        [HttpPost, ActionName("DeleteFilm")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            var FilmId = int.Parse(Request["FilmId"]);
            filmService.Delete(f => f.FilmId == FilmId);
            filmService.Commit();
            return RedirectToAction("Index");
        }
        #endregion

        #region Category
        // GET: CategoryViewModels/Details/5
        public ActionResult DetailsCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorySevice.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
           
            
            return View("Category/Details", category);
        }

        // GET: CategoryViewModels/Create
        public ActionResult CreateCategory()
        {
            return View("Category/Create");
        }

        // POST: CategoryViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryId,CategoryName")] Category categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                categorySevice.Add(categoryViewModel);
                categorySevice.Commit();
                return RedirectToAction("Index");
            }

            return View(categoryViewModel);
        }

        // GET: CategoryViewModels/Edit/5
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorySevice.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            
            return View("Category/Edit",category);
        }

        // POST: CategoryViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "CategoryId,CategoryName")] Category categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                Category c = categorySevice.GetById(categoryViewModel.CategoryId);
                c.CategoryName = categoryViewModel.CategoryName;
                categorySevice.Update(c);
                categorySevice.Commit();
                //foreach (var item in c.Films)
                //{
                //    item.Categories = filmService.GetFilmCategories(item.FilmId).ToList();
                //}
                return RedirectToAction("Index");
            }
            return View("Category/Edit",categoryViewModel);
        }

        // GET: CategoryViewModels/Delete/5
        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorySevice.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
          
            return View("Category/Delete",category);
        }

        // POST: CategoryViewModels/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCatConfirmed(int id)
        {
            Category category = categorySevice.GetById(id);
            categorySevice.Delete(category);
            categorySevice.Commit();
            return RedirectToAction("Index");
        }

        #endregion

        #region Acteur
        // GET: Acteurs/Details/5
        public ActionResult DetailsActeur(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acteur acteur = acteurSevice.GetById(id.Value);
            if (acteur == null)
            {
                return HttpNotFound();
            }
     
            return View("Acteur/Details",acteur);
        }

        // GET: Acteurs/Create
        public ActionResult CreateActeur()
        {
            return View("Acteur/Create");
        }

        // POST: Acteurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateActeur([Bind(Include = "ActeurId,NomActeur,PrenomActeur,NoteActeur")] Acteur acteur)
        {
            if (ModelState.IsValid)
            {
                acteurSevice.Add(acteur);
                acteurSevice.Commit();

                return RedirectToAction("Index");
            }

            return View("Acteur/Create",acteur);
        }

        // GET: Acteurs/Edit/5
        public ActionResult EditActeur(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acteur acteur = acteurSevice.GetById(id.Value);
            if (acteur == null)
            {
                return HttpNotFound();
            }
         
            return View("Acteur/Edit",acteur);
        }

        // POST: Acteurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActeur([Bind(Include = "ActeurId,NomActeur,PrenomActeur,NoteActeur")] Acteur acteur)
        {
            if (ModelState.IsValid)
            {
                Acteur a = acteurSevice.GetById(acteur.ActeurId);
                a.NomActeur = acteur.NomActeur;
                a.NoteActeur = acteur.NoteActeur;
                a.PrenomActeur = acteur.PrenomActeur;
                acteurSevice.Update(a);
                acteurSevice.Commit();
                return RedirectToAction("Index");
            }
            return View("Acteur/Edit",acteur);
        }

        // GET: Acteurs/Delete/5
        public ActionResult DeleteActeur(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acteur acteur = acteurSevice.GetById(id.Value);
            if (acteur == null)
            {
                return HttpNotFound();
            }
            
            return View("Acteur/Delete",acteur);
        }

        // POST: Acteurs/Delete/5
        [HttpPost, ActionName("DeleteActeur")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteActConfirmed(int id)
        {
            Acteur acteur = acteurSevice.GetById(id);
            acteurSevice.Delete(acteur);
            acteurSevice.Commit();
            return RedirectToAction("Index");
        }

        #endregion

        #region Projection

        // GET: Projections/Details/5
        public ActionResult DetailsProjection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projection projection = projectionSevice.GetById(id.Value);
            if (projection == null)
            {
                return HttpNotFound();
            }
           
            return View("Projection/Details",projection);
        }

        // GET: Projections/Create
        public ActionResult CreateProjection()
        {
            ViewBag.FilmId = new SelectList(filmService.GetMany(), "FilmId", "FilmTitle");
            ViewBag.SalleId = new SelectList(salleSevice.GetMany(), "SalleId", "NomSalle");
            return View("Projection/Create");
        }

        // POST: Projections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProjection([Bind(Include = "ProjectionId,DateProjection,SalleId,FilmId")] Projection projection)
        {
            if (ModelState.IsValid)
            {
                projectionSevice.Add(projection);
                projectionSevice.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.FilmId = new SelectList(filmService.GetMany(), "FilmId", "FilmTitle", projection.FilmId);
            ViewBag.SalleId = new SelectList(salleSevice.GetMany(), "SalleId", "NomSalle", projection.SalleId);
            return View("Projection/Create", projection);
        }

        // GET: Projections/Edit/5
        public ActionResult EditProjection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projection projection = projectionSevice.GetById(id.Value);
            if (projection == null)
            {
                return HttpNotFound();
            }
         
            ViewBag.FilmId = new SelectList(filmService.GetMany(), "FilmId", "FilmTitle", projection.FilmId);
            ViewBag.SalleId = new SelectList(salleSevice.GetMany(), "SalleId", "NomSalle", projection.SalleId);
            return View("Projection/Edit",projection);
        }

        // POST: Projections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProjection([Bind(Include = "ProjectionId,DateProjection,SalleId,FilmId")] Projection projection)
        {
            if (ModelState.IsValid)
            {
                Projection p =projectionSevice.GetById(projection.ProjectionId);
                p.DateProjection = projection.DateProjection;
                p.FilmId = projection.FilmId;
                p.SalleId = projection.SalleId;
                projectionSevice.Update(p);
                projectionSevice.Commit();

                p.Film = filmService.GetById(p.FilmId);
                p.Salle = salleSevice.GetById(p.SalleId);
                return RedirectToAction("Index");
            }
            ViewBag.FilmId = new SelectList(filmService.GetMany(), "FilmId", "FilmTitle", projection.FilmId);
            ViewBag.SalleId = new SelectList(salleSevice.GetMany(), "SalleId", "NomSalle", projection.SalleId);
            return View("Projection/Edit", projection);
        }

        // GET: Projections/Delete/5
        public ActionResult DeleteProjection(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projection projection = projectionSevice.GetById(id.Value);
            if (projection == null)
            {
                return HttpNotFound();
            }
           
            return View("Projection/Delete", projection);
        }

        // POST: Projections/Delete/5
        [HttpPost, ActionName("DeleteProjection")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProjConfirmed(int id)
        {
            Projection projection = projectionSevice.GetById(id);
            projectionSevice.Delete(projection);
            projectionSevice.Commit();
            return RedirectToAction("Index");
        }
        #endregion

        #region Salle
        
        // GET: Salles/Details/5
        public ActionResult DetailsSalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle =salleSevice.GetById(id.Value);
            if (salle == null)
            {
                return HttpNotFound();
            }
         
            return View("Salle/Details",salle);
        }

        // GET: Salles/Create
        public ActionResult CreateSalle()
        {
            return View("Salle/Create");
        }

        // POST: Salles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSalle([Bind(Include = "SalleId,NomSalle,AdresseSalle")] Salle salle)
        {
            if (ModelState.IsValid)
            {
               salleSevice.Add(salle);
                salleSevice.Commit();
                return RedirectToAction("Index");
            }

            return View("Salle/Create", salle);
        }

        // GET: Salles/Edit/5
        public ActionResult EditSalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle = salleSevice.GetById(id.Value);
            if (salle == null)
            {
                return HttpNotFound();
            }
            return View("Salle/Edit",salle);
        }

        // POST: Salles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSalle([Bind(Include = "SalleId,NomSalle,AdresseSalle")] Salle salle)
        {
            if (ModelState.IsValid)
            {
                Salle s = salleSevice.GetById(salle.SalleId);
                s.NomSalle = salle.NomSalle;
                s.AdresseSalle = salle.AdresseSalle;
                salleSevice.Update(s);
                salleSevice.Commit();
                return RedirectToAction("Index");
            }
            return View("Salle/Edit", salle);
        }

        // GET: Salles/Delete/5
        public ActionResult DeleteSalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Salle salle = salleSevice.GetById(id.Value);
            if (salle == null)
            {
                return HttpNotFound();
            }
            
            return View("Salle/Delete",salle);
        }

        // POST: Salles/Delete/5
        [HttpPost, ActionName("DeleteSalle")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSalConfirmed(int id)
        {
            Salle salle = salleSevice.GetById(id);
            salleSevice.Delete(salle);
            salleSevice.Commit();
            return RedirectToAction("Index");
        }



        #endregion

        #region Event
        // GET: Events/Details/5
        public ActionResult DetailsEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = eventService.GetById(id.Value);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View("Event/Details",@event);
        }

        // GET: Events/Create
        public ActionResult CreateEvent()
        {
            return View("Event/Create");
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEvent([Bind(Include = "EventId,DateDebut,DateFin,Description,Titre,Adresse,Capacite")] Event @event)
        {
            if (ModelState.IsValid)
            {
               eventService.Add(@event);
                eventService.Commit();
                return RedirectToAction("Index");
            }

            return View("Event/Create",@event);
        }

        // GET: Events/Edit/5
        public ActionResult EditEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = eventService.GetById(id.Value);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View("Event/Edit", @event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEvent([Bind(Include = "EventId,DateDebut,DateFin,Description,Titre,Adresse,Capacite")] Event @event)
        {
            if (ModelState.IsValid)
            {
                Event e = eventService.GetById(@event.EventId);
                e.DateDebut = @event.DateDebut;
                e.DateFin = @event.DateFin;
                e.Description = @event.Description;
                e.Capacite = @event.Capacite;
                e.Adresse = @event.Adresse;
                e.Titre = @event.Titre;

                eventService.Update(e);
                eventService.Commit();
                return RedirectToAction("Index");
            }
            return View("Event/Edit",@event);
        }

        // GET: Events/Delete/5
        public ActionResult DeleteEvent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = eventService.GetById(id.Value);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View("Event/Delete",@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("DeleteEvent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteEventConfirmed(int id)
        {
            Event @event = eventService.GetById(id);
            eventService.Delete(@event);
            eventService.Commit();
            return RedirectToAction("Index");
        }

        #endregion

        #region Participation


        // GET: Participations/Details/5
        public ActionResult DetailsParticipation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participation participation = participationService.GetById(id.Value);
            if (participation == null)
            {
                return HttpNotFound();
            }
            return View("Participation/Details", participation);
        }

        // GET: Participations/Create
        public ActionResult CreateParticipation()
        {
            ViewBag.EventId = new SelectList(eventService.GetMany(), "EventId", "Titre");
            ViewBag.UserId = new SelectList(userService.GetMany(), "UserId", "Username");
            return View("Participation/Create");
        }

        // POST: Participations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateParticipation([Bind(Include = "ParticipationId,EventId,UserId")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                Event e =eventService.GetById(participation.EventId);
                if (e.Capacite > 0)
                {
                    Participation p = new Participation();
                    p.EventId = participation.EventId;
                    p.UserId = participation.UserId;
                    participationService.Add(p);
                    participationService.Commit();

                    e.Capacite--;
                    eventService.Update(e);
                    eventService.Commit();
                    return RedirectToAction("Index");
                }
                else
                {

                    ViewBag.msg = "pas de places";

                }




            }

            ViewBag.EventId = new SelectList(eventService.GetMany(), "EventId", "Titre", participation.EventId);
            ViewBag.UserId = new SelectList(userService.GetMany(), "UserId", "Username", participation.UserId);
            return View("Participation/Create", participation);
        }

        // GET: Participations/Edit/5
        public ActionResult EditParticipation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participation participation = participationService.GetById(id.Value);
            if (participation == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(eventService.GetMany(), "EventId", "Titre", participation.EventId);
            ViewBag.UserId = new SelectList(userService.GetMany(), "UserId", "Username", participation.UserId);
            return View("Participation/Edit", participation);
        }

        // POST: Participations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditParticipation([Bind(Include = "ParticipationId,EventId,UserId")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                Participation p= participationService.GetById(participation.ParticipationId);
                p.UserId = participation.UserId;
                p.EventId = participation.EventId;

                participationService.Update(p);
                participationService.Commit();

                p.Event = eventService.GetById(p.EventId);
                p.User = userService.GetById(p.UserId);
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(eventService.GetMany(), "EventId", "Titre", participation.EventId);
            ViewBag.UserId = new SelectList(userService.GetMany(), "UserId", "Username", participation.UserId);
            return View("Participation/Edit", participation);
        }

        // GET: Participations/Delete/5
        public ActionResult DeleteParticipation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Participation participation = participationService.GetById(id.Value);
            if (participation == null)
            {
                return HttpNotFound();
            }
            return View("Participation/Delete", participation);
        }

        // POST: Participations/Delete/5
        [HttpPost, ActionName("DeleteParticipation")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteParticipationConfirmed(int id)
        {
            Participation participation = participationService.GetById(id);
            participationService.Delete(participation);
            participationService.Commit();

            Event e =eventService.GetById(participation.EventId);
            e.Capacite++;
            eventService.Update(e);
            eventService.Commit();
            return RedirectToAction("Index");
        }
        #endregion

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
                else
                {
                    break;
                }

            }

            return View("Statestique/StatFilmParNote", models);
        }
        public ActionResult StatFilmParCat (){

            List<Category> cts = categorySevice.GetMany().ToList();
            List<dynamic> models = new List<dynamic>();
            foreach (var item in cts)
            {
                dynamic x = new
                {
                    label = item.CategoryName,
                    value = filmService.GetFilmByCategory(item.CategoryId).Count()
                };

                models.Add(x);
            }

            return View("Statestique/StatFilmParCat", models);
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
        public ActionResult StatEventPartiicipation()
        {

            List<Event> cts = eventService.GetMany().ToList();
            List<dynamic> models = new List<dynamic>();
            foreach (var item in cts)
            {
                string etat="Finish" ;

                if (item.DateFin > DateTime.Now && item.DateDebut<DateTime.Now)
                {
                    etat = "In process";
                }
                else if(item.DateDebut > DateTime.Now)
                {
                    etat = "Futur";
                }
                dynamic x = new
                {
                    tt = item.Titre,
                    id=item.EventId,
                    et=etat,
                    adr=item.Adresse.FormattedAdress,
                    cap=item.Capacite,
                    part = participationService.GetMany(p=>p.EventId==item.EventId).Count()
                };

                models.Add(x);
            }

            return View("Statestique/StatEvent", models);
        }



        #endregion

        #region User
     

        // GET: Users/Details/5
        public ActionResult DetailsUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userService.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("User/Details", user);
        }

        // GET: Users/Create
        public ActionResult CreateUser()
        {
            ViewBag.RoleId = new SelectList(roleService.GetMany(), "RoleId", "RoleName");
            return View("User/Create");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser([Bind(Include = "UserId,Username,Email,Password,FirstName,LastName,IsActive,CreateDate")] User user)
        {
            if (ModelState.IsValid)
            {

                int roleId =int.Parse( Request["RoleId"]);
                WebContext ctx = new WebContext();
                User u = new User();
                u.Username = user.Username;
                u.Email = user.Email;
                u.FirstName = user.FirstName;
                u.Password = user.Password;
                u.CreateDate = DateTime.Now;
                u.IsActive = false;
                var roleuser = ctx.Roles.Single(r => r.RoleId == roleId);
                u.Roles = new List<Role>();
                u.Roles.Add(roleuser);
                ctx.Users.Add(u);
                ctx.SaveChanges();
                //userService.Add(user);
                //userService.Commit();


                return RedirectToAction("Index");
            }

            return View("User/Create", user);
        }

        // GET: Users/Edit/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userService.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("User/Edit", user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "UserId,Username,Email,FirstName,LastName,IsActive,CreateDate")] User user)
        {
            if (ModelState.IsValid)
            {


                User usr = userService.GetById(user.UserId);
                usr.Username = user.Username;
                usr.FirstName = user.FirstName;
                usr.LastName = user.LastName;
                usr.Email = user.Email;
                usr.IsActive = user.IsActive;
                usr.CreateDate = user.CreateDate;
                userService.Update(usr);
                userService.Commit();
                
                return RedirectToAction("Index");
            }
            return View("User/Edit",user);
        }

        // GET: Users/Delete/5
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userService.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("User/Delete", user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUser(int id)
        {
            User user = userService.GetById(id);
            userService.Delete(user);
            userService.Commit();
            return RedirectToAction("Index");
        }


        #endregion
        #region role
       

        // GET: Roles/Details/5
        public ActionResult DetailsRole(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = ww.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("Role/Details",role);
        }

        // GET: Roles/Create
        public ActionResult CreateRole()
        {
            return View("Role/Create");
        }

        // POST: Roles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole([Bind(Include = "RoleId,RoleName,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                ww.Roles.Add(role);
                ww.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Role/Create",role);
        }

        // GET: Roles/Edit/5
        public ActionResult EditRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = ww.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("Role/Edit",role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole([Bind(Include = "RoleId,RoleName,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                ww.Entry(role).State = EntityState.Modified;
                ww.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Role/Edit",role);
        }

        // GET: Roles/Delete/5
        public ActionResult DeleteRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = ww.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View("Role/Delete",role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Role role = ww.Roles.Find(id);
            ww.Roles.Remove(role);
            ww.SaveChanges();
            return RedirectToAction("Index");
        }

        WebContext ww = new WebContext();
        #endregion

        #region Reclamation
        public ActionResult DetailsReclamation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reclamation reclamation = reclamationService.GetById(id.Value);
            if (reclamation == null)
            {
                return HttpNotFound();
            }
            return View("Reclamation/Details",reclamation);
        }

       // [OutputCache(Duration = 999999)]
        public virtual JavaScriptResult Message()
        {
            var messages = reclamationService.GetMany(x => x.état.Equals("non lu")).ToList();
            var ser = new JavaScriptSerializer();
            var script = @"var message="+ser.Serialize(messages);
            return JavaScript(script);
        }
        [HttpPost]
        public ActionResult Mail()
        {
            
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("oumayma.dridi@esprit.tn ");
                mail.Subject = "Reply to your reclamation";
                mail.Body = Request["message"];

                mail.To.Add(Request["email"]);
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Credentials = new System.Net.NetworkCredential("oumayma.dridi@esprit.tn ", "esprit2013");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            int recId = int.Parse(Request["rec"]);
               Reclamation c= reclamationService.GetById(recId);
            c.état = "lu";
            reclamationService.Update(c);
            reclamationService.Commit();
            ViewBag.success = "email sent";
            return RedirectToAction("Index");
            }
           
        }
        #endregion


    }


