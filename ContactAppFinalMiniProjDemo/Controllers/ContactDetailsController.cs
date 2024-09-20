using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ContactAppFinalMiniProjDemo.Data;
using ContactAppFinalMiniProjDemo.Models;
using NHibernate.Linq;

namespace ContactAppFinalMiniProjDemo.Controllers
{
    [Authorize(Roles = "Staff")]
    public class ContactDetailsController : Controller
    {
        // GET: ContactDetails
        //getting contactdetails by contact id
        public ActionResult Index(Guid contactId)
        {
            TempData["ContactId"] = contactId;  //storing the contact id in the temp data 
            return View();
        }

        public ActionResult GetContactDetails(int page, int rows, string sidx, string sord, bool _search, string searchField, string searchString, string searchOper)
        {
            Guid contactId = (Guid)TempData.Peek("ContactId");
            using (var session = NHibernateHelper.CreateSession())
            {
                var contactDetails = session.Query<ContactDetails>().Where(cd => cd.Contact.Id == contactId).ToList();
                var contactDetailsForSearch = contactDetails;

                if (_search && searchField == "Email" && searchOper == "eq")
                {
                    contactDetailsForSearch = contactDetails.Where(p => p.PhoneNumber.ToString() == searchString).ToList();  //equality search by name
                }
                //get total count of records (for pagination)

                int totalCount = contactDetails.Count();

                //calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalCount / rows);

                //for sorting

                switch (sidx)
                {
                    case "PhoneNumber":
                        contactDetails = sord == "asc" ? contactDetails.OrderBy(p => p.PhoneNumber).ToList()
                            : contactDetails.OrderByDescending(p => p.PhoneNumber).ToList();
                        break;

                    case "Email":
                        contactDetails = sord == "asc" ? contactDetails.OrderBy(p => p.Email).ToList()
                            : contactDetails.OrderByDescending(p => p.Email).ToList();
                        break;

                    default:
                        break;
                }

                var jsonData = new
                {
                    total = totalPages,
                    page,
                    records = totalCount,
                    rows = contactDetailsForSearch.Select(ContactDetail => new
                    {
                        cell = new string[]
                        {
                            ContactDetail.Id.ToString(),
                            ContactDetail.PhoneNumber.ToString(),
                            ContactDetail.Email
                        }
                    }).Skip((page - 1) * rows).Take(rows).ToArray()
                //    rows = (from contactDetail in contactDetails
                //            orderby sidx + " " + sord
                //            select new
                //            {
                //                cell = new string[]
                //            {
                //                contactDetail.Id.ToString(),
                //                contactDetail.PhoneNumber.ToString(),
                //                contactDetail.Email
                //            }
                //            }).Skip((page - 1) * rows).Take(rows).ToArray()


                    };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Add(ContactDetails contactDetails)
        {
            //getting contactid for adding contactdetails of that particular contact
            //already stored the contact id in the tempdata 
            Guid contactId = (Guid)TempData.Peek("ContactId"); //retrieving the contact id 
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var contact = session.Query<Contact>().SingleOrDefault(c => c.Id == contactId);
                        // Assign the contact to the contact details
                        contactDetails.Contact = contact;

                        session.Save(contactDetails);
                        transaction.Commit();
                        return Json(new { success = true, message = "Contact Detail added successfully" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Error adding contactDetails : " + ex.Message);
                    }

                }
            }

        }

       
        public ActionResult Delete(Guid id)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var contactDetailToDel = session.Query<ContactDetails>().FirstOrDefault(cd => cd.Id == id);
                        if (contactDetailToDel != null)
                        {
                            session.Delete(contactDetailToDel);
                            transaction.Commit();
                            return Json(new { success = true, message = "Contact Detail deleted successfully" });
                        }
                        return Json(new { success = false, message = "Contact Detail not found" });
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Error deleting contactDetails : " + ex.Message);
                    }
                }
            }

        }

        public ActionResult Edit(ContactDetails contactDetails)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        //getting contactdetail
                        var contactDetailToEdit = session.Query<ContactDetails>().FirstOrDefault(cd => cd.Id == contactDetails.Id);
                        if (contactDetailToEdit != null)
                        {
                            contactDetailToEdit.PhoneNumber = contactDetails.PhoneNumber;
                            contactDetailToEdit.Email = contactDetails.Email;
                            session.Update(contactDetailToEdit);
                            transaction.Commit();
                            return Json(new { success = true, message = "Contact Detail edited successfully" });
                        }
                        return Json(new { success = false, message = "Contact Detail not found" });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new HttpStatusCodeResult(500, "Internal server error ");
                    }
                }
            }
        }

    }
}