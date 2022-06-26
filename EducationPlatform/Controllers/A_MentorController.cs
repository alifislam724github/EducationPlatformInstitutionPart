using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationPlatform.Models;

namespace EducationPlatform.Controllers
{
    public class A_MentorController : Controller
    {
        // GET: A_Mentor
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult MentorAdd()
        {
            return View();
        }
        [HttpPost]

        public ActionResult MentorAdd(Mentor obj)
        {
            var db = new EducationPlatformEntities();

            string instituteEmail = Session["instituteEmail"].ToString();

            var y=(from i in db.Institutions where i.Email== instituteEmail select
                   i.Name).FirstOrDefault();
            var x= new Mentor() { Name=obj.Name, Address=obj.Address, Email=obj.Email, 
                Phone=obj.Phone, Password=obj.Password, Gender=obj.Gender,
            Institution=y, IsValid="Yes"};
            
            db.Mentors.Add(x);
            db.SaveChanges();
            return RedirectToAction("Index", "A_Institution");
        }
        public ActionResult MentorList()
        {
            var db = new EducationPlatformEntities();

            string instituteEmail = Session["instituteEmail"].ToString();

            var InstituteName = (from i in db.Institutions
                     where i.Email == instituteEmail
                     select i.Name).FirstOrDefault();

            var mentorList = (from y in db.Mentors where y.Institution == InstituteName
                     select y).ToList();

            //var mentor = db.Mentors.ToList();
            return View(mentorList);
        }

        public ActionResult MentorDelete(int Id)
        {
            var db = new EducationPlatformEntities();
            var mentor = (from p in db.Mentors where p.Id == Id select p).SingleOrDefault();
            db.Mentors.Remove(mentor);
            db.SaveChanges();
            return RedirectToAction("MentorList");
        }
    }
}
    