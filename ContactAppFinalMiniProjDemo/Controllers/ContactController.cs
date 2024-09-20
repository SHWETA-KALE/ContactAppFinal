﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactAppFinalMiniProjDemo.Data;
using ContactAppFinalMiniProjDemo.DTOs;
using ContactAppFinalMiniProjDemo.Models;
using Newtonsoft.Json;
using NHibernate.Linq;

namespace ContactAppFinalMiniProjDemo.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //showing contacts of that particular logged in user
        // GET: Contact
        public ActionResult GetContacts()
        {


            // Check if the user is logged in
            //if (Session["UserId"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}
            //Guid userId = (Guid)Session["UserId"]; // Retrieve userId from the session
            //using (var session = NHibernateHelper.CreateSession())
            //{
            //    // Eager load User and Contacts to avoid lazy loading outside the session
            //    var user = session.Query<User>()
            //                      .FetchMany(u => u.Contacts) // Ensure Contacts are fetched eagerly
            //                      .FirstOrDefault(u => u.Id == userId);

            //    if (user == null)
            //    {
            //        return HttpNotFound();
            //    }

            //    var contacts = user.Contacts.Select(c => new Contact
            //    {
            //        Id = c.Id,
            //        FName = c.FName,
            //        LName = c.LName,
            //        IsActive = c.IsActive
            //    }).ToList();

            //    return Json(contacts, JsonRequestBehavior.AllowGet);



            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            Guid userId = (Guid)Session["UserId"];
            using (var session = NHibernateHelper.CreateSession())
            {
                //var contacts = session.Query<Contact>().Fetch(c=>c.User).Where(c => c.User.Id == userId).ToList();
                //if (contacts.Count > 0)
                //{
                //    return Json(contacts,JsonRequestBehavior.AllowGet);
                //}
                var userWithContacts = session.Query<User>().FetchMany(u => u.Contacts).SingleOrDefault(u => u.Id == userId);

                if (userWithContacts != null && userWithContacts.Contacts.Count > 0)
                {
                    // Map entities to DTOs
                    var contactsDto = userWithContacts.Contacts.Select(contact => new ContactDTO
                    {
                        Id = contact.Id,
                        FName = contact.FName,
                        LName = contact.LName,
                        IsActive = contact.IsActive
                    }).ToList();

                    return Json(contactsDto, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(500);
            }
        }


        //public ActionResult Add(Contact contact)
        //{

        //}

        [HttpPost]

        public ActionResult Add(Contact contact)
        {
            if (Session["UserId"] == null)
            {
                return new HttpStatusCodeResult(401, "Unauthorized");
            }

            Guid userId = (Guid)Session["UserId"];

            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // Fetch the user from the database
                        var user = session.Query<User>().SingleOrDefault(u => u.Id == userId);

                        if (user == null)
                        {
                            return HttpNotFound("User not found");
                        }

                        contact.IsActive = true;

                        // Associate the new contact with the logged-in user
                        contact.User = user;

                        // Save the contact to the database
                        session.Save(contact);
                        transaction.Commit();

                        return Json(new
                        {
                            Id = contact.Id,
                            FName = contact.FName,
                            LName = contact.LName,
                            IsActive = contact.IsActive
                        });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Error adding contact: " + ex.Message);
                    }
                }
            }
        }

        //[HttpPost]
        //public ActionResult Edit(Contact contact)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return new HttpStatusCodeResult(401, "Unauthorized");
        //    }
        //    Guid userId = (Guid)Session["UserId"];

        //    using(var session = NHibernateHelper.CreateSession())
        //    {
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            var existingContact = session.Query<Contact>().SingleOrDefault(c=>c.Id == contact.Id);
        //            if(existingContact != null)
        //            {
        //                existingContact.FName = contact.FName;
        //                existingContact.LName = contact.LName;
        //                session.Update(existingContact);
        //                transaction.Commit();
        //            }
        //            return Json(existingContact);
        //        }
        //    }
        //}

        // GET: Contact/GetContact/5
        //public ActionResult GetContact(Guid id)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return new HttpStatusCodeResult(401, "Unauthorized");
        //    }

        //    using (var session = NHibernateHelper.CreateSession())
        //    {
        //        var contact = session.Query<Contact>().SingleOrDefault(c => c.Id == id);
        //        if (contact == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return Json(contact, JsonRequestBehavior.AllowGet);
        //    }
        //}

        // POST: Contact/Edit
        //[HttpPost]
        //public ActionResult Edit(Contact contact)
        //{
        //    if (Session["UserId"] == null)
        //    {
        //        return new HttpStatusCodeResult(401, "Unauthorized");
        //    }

        //    using (var session = NHibernateHelper.CreateSession())
        //    {
        //        using (var transaction = session.BeginTransaction())
        //        {
        //            var existingContact = session.Query<Contact>().SingleOrDefault(c => c.Id == contact.Id);
        //            if (existingContact != null)
        //            {
        //                existingContact.FName = contact.FName;
        //                existingContact.LName = contact.LName;
        //                // You may want to include additional fields if necessary

        //                session.Update(existingContact);
        //                transaction.Commit();
        //            }
        //            else
        //            {
        //                return HttpNotFound();
        //            }

        //            return Json(existingContact); // Or another appropriate result
        //        }
        //    }
        //}



        [HttpPost]
        // Soft delete contact
        public ActionResult UpdateContactStatus(Guid contactId, bool isActive)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // Fetch the contact by contactId
                    var contact = session.Query<Contact>().FirstOrDefault(c => c.Id == contactId);
                    if (contact != null)
                    {
                        contact.IsActive = isActive; // Soft delete by setting IsActive to false
                        session.Update(contact); // Update the contact in the database
                        transaction.Commit();
                        return Json(new { success = true });
                    }
                    return Json(new { success = false, message = "Contact not found." });
                }
            }
        }

        


    }
}

    

