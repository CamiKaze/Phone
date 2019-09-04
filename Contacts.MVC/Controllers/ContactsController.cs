using Contacts.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Contacts.MVC.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index()
        {
            // IEnumerable describes behavior, while List is an implementation of that behavior. When you use
            // IEnumerable, you give the compiler a chance to defer work until later, possibly optimizing along the way.
            // If you use ToList() you force the compiler to reify the results right away. Whenever I'm "stacking"
            // LINQ expressions, I use IEnumerable, because by only specifying the behavior I give LINQ a chance to
            // defer evaluation and possibly optimize the program. 
            IEnumerable<ContactsModel> conList;
            HttpResponseMessage response = GlobalVariables.ContactsAPIClient.GetAsync("Contacts").Result;
            // We take the response from the API and store it in an IEnumerable
            conList = response.Content.ReadAsAsync<IEnumerable<ContactsModel>>().Result;
            // We can now pass it on to the Index view
            return View(conList);
        }

        public ActionResult Search(string id)
        {
            HttpResponseMessage response = GlobalVariables.ContactsAPIClient.GetAsync("Contacts/" + id).Result;
            if (response.StatusCode.ToString() == "NotFound")
            {
                TempData["ErrorMessage"] = "No record found!";
                return RedirectToAction("index");
            }
            else
                return View(response.Content.ReadAsAsync<ContactsModel>().Result);
        }

        public ActionResult AddOrEdit(int id = 0, string name = null)
        {// (1) Insert operation => (2) AddOrEdit.cshtml
            if (id == 0)
              return View(new ContactsModel());
            else
            {
                HttpResponseMessage response = GlobalVariables.ContactsAPIClient.GetAsync("Contacts/"+ id.ToString()).Result;
                return View(response.Content.ReadAsAsync<ContactsModel>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(ContactsModel con)
        {// (1) Insert operation => (2) AddOrEdit.cshtml
         if (con.ContactID == 0)
            {
                HttpResponseMessage response = GlobalVariables.ContactsAPIClient.PostAsJsonAsync("Contacts", con).Result;
                TempData["SuccessMessage"] = "Contact Saved";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.ContactsAPIClient.PutAsJsonAsync("Contacts/"+ con.ContactID, con).Result;
                TempData["SuccessMessage"] = "Contact Updated";
            }

            return RedirectToAction("index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.ContactsAPIClient.DeleteAsync("Contacts/" +id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted";
            return RedirectToAction("index");
        }
    }
}