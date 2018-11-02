
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using Web.DAL;
using Web.Models;
using Web.Security;
using System.Threading.Tasks;
using Domain.Entity;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        WebContext Context = new WebContext();
        //
     

        [HttpPost]
        public ActionResult Login()
        {
            string email = Request["email"];
            string password = Request["pass"];
                var user = Context.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefault();
                if (user != null)
                {
                    var roles=user.Roles.Select(m => m.RoleName).ToArray();

                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.UserId = user.UserId;
                    serializeModel.FirstName = user.FirstName;
                    serializeModel.LastName = user.LastName;
                    serializeModel.roles = roles;
                    

                   string userData = JsonConvert.SerializeObject(serializeModel);
                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             user.Email,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(15),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);

                   

                    if(roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.Contains("User"))
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                HomeController.msg= "login-e";


            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

 
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
       
        public  ActionResult Register()
        {
            string email = Request["email"];
            string password = Request["pass"];
            string confpassword = Request["confpass"];
            string Username = Request["company"];
            string fname = Request["fname"];
            string lname = Request["lname"];

            User u = new User();
                u.Username = Username;
                u.Email = email;
                u.FirstName = fname;
                u.Password = password;
                u.CreateDate = DateTime.Now;
                u.IsActive = false;
                var roleuser = Context.Roles.Single(r=>r.RoleId == 2);
                u.Roles = new List<Role>();
                u.Roles.Add(roleuser);
                Context.Users.Add(u);
                Context.SaveChanges();


            //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //        // Send an email with this link
            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //        return View("DisplayEmail");

            HomeController.msg = "regiter-s";
            return RedirectToAction("Index", "Home");
           



        }
    }
}