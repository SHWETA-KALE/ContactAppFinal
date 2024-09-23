using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppFinalMiniProjDemo.Data;
using ContactAppFinalMiniProjDemo.Models;
using System.Web.Security;
using System.Web.UI.WebControls;
using ContactAppFinalMiniProjDemo.ViewModels;
using NHibernate.Linq;

namespace ContactAppFinalMiniProjDemo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var users = session.Query<User>().Where(u => u.IsAdmin == false).ToList();
                return View(users);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM loginVM)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    if (ModelState.IsValid)
                    {
                        var user = session.Query<User>().FirstOrDefault(u => u.UserName == loginVM.UserName);
                        if (user != null)
                        {
                            if (user.IsActive)
                            {
                                if (PasswordHelper.VerifyPassword(loginVM.Password, user.Password))
                                {
                                    FormsAuthentication.SetAuthCookie(loginVM.UserName, true); //true indicates persistent cookie
                                    ////storing the user id in the session for contacts and contact details
                                    Session["UserId"] = user.Id;
                                    if (user.IsAdmin)
                                    {
                                        return RedirectToAction("Index");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Contact");
                                    }
                                }
                            }

                        }
                    }
                }
                ModelState.AddModelError("", "UserName/Password doesn't match");
                return View();
            }

        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {

                    user.Role.User = user;
                    user.Password = PasswordHelper.HashPassword(user.Password);

                    if (user.IsAdmin)
                    {
                        user.Role.RoleName = "Admin";
                    }
                    else
                    {
                        user.Role.RoleName = "Staff";
                    }
                    session.Save(user);
                    transaction.Commit();
                }
                return RedirectToAction("Login");


            }
        }

        [HttpGet]
        public ActionResult Edit(Guid userid)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var userToEdit = session.Query<User>().FirstOrDefault(u => u.Id == userid);
                    return View(userToEdit);
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var userToEdit = session.Query<User>().FirstOrDefault(u => u.Id == user.Id);
                    if (userToEdit != null)
                    {
                        userToEdit.UserName = user.UserName;
                        userToEdit.Password = user.Password;
                        userToEdit.FName = user.FName;
                        userToEdit.LName = user.LName;
                        userToEdit.Email = user.Email;
                        session.Update(userToEdit);
                        transaction.Commit();
                    }
                }

                return RedirectToAction("Index");
            }
        }


        [HttpGet]
        public ActionResult ViewAllAdmins()
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var admins = session.Query<User>().Where(u => u.IsAdmin == true).ToList();
                    return View(admins);
                }
            }
        }

        [HttpPost]
        //soft delete 
        public ActionResult UpdateUserStatus(Guid id, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    //fetch the user by id
                    var user = session.Query<User>().FirstOrDefault(u => u.Id == id);
                    if (user != null)
                    {
                        user.IsActive = isActive;
                        session.Update(user); // Update the user in the database
                        transaction.Commit();
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, message = "User not found." });
                }
            }


        }

        [HttpGet]
        public ActionResult ViewContacts(Guid userid)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var userWithContacts = session.Query<User>().FirstOrDefault(u => u.Id == userid);

                if (userWithContacts == null)
                {
                    return HttpNotFound("User not found.");
                }

                var contacts = userWithContacts.Contacts.ToList();
                return View(contacts);
            }
        }


        [HttpGet]
        public ActionResult ContactDetails(Guid contactid)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                var Contacts = session.Query<Contact>().FirstOrDefault(u => u.Id == contactid);
                if (Contacts == null)
                {
                    return HttpNotFound("User not found");
                }
                var contactDetails = Contacts.ContactDetails.ToList();
                return View(contactDetails);
            }
        }



        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }


    }
}

