using Data;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class FileController : BaseController
    {
        CinemaContext db = new CinemaContext();
        // GET: File

        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
        [HttpPost]
        public ActionResult UploadeImage( HttpPostedFileBase upload)
        {
            var FilmId= Request["FilmId"];
            var RedirectUrl = Request["RedirectUrl"];
            if (upload != null && upload.ContentLength > 0)
            {
                var photo = new File
                {
                    FileName = System.IO.Path.GetFileName(upload.FileName),
                    FileType = FileType.Photo,
                    ContentType = upload.ContentType
                };
                using (var reader = new System.IO.BinaryReader(upload.InputStream))
                {
                    photo.Content = reader.ReadBytes(upload.ContentLength);
                }
                photo.FilmId =int.Parse(FilmId);
                db.Files.Add(photo);
                db.SaveChanges();
            }
            return Redirect(RedirectUrl);
        }

        [HttpPost]
        public ActionResult DeleteFile()
        {
            var FileId = Request["FileId"];
            var RedirectUrl = Request["RedirectUrl"];
            File film = db.Files.Find(int.Parse(FileId));
            db.Files.Remove(film);
            db.SaveChanges();
            return Redirect(RedirectUrl);
        }
    }
}