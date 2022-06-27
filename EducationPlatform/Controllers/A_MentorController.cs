using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using EducationPlatform.Auth;
using EducationPlatform.Models;

namespace EducationPlatform.Controllers
{
    [InstitutionLogged]
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

            MailMessage mail = new MailMessage();
            mail.To.Add(obj.Email);
            mail.From = new MailAddress("19-40135-1@student.aiub.edu");
            mail.Subject = "Profile created in ABC Education";
            string Body = "Congratulations!! <br/>" +
                           "Your profile has been added <br/>" +
                           "Your email:" + obj.Email + "<br/>" +
                           "Your password:" + obj.Password + "<br/>" +
                           "Please use this email and password to login" + "<br/>" +
                           "<br/>" +
                           "<b>Best Wishes</b><br/>" +
                           
                           y;

            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp-mail.outlook.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("19-40135-1@student.aiub.edu", "Honest9016*"); // Enter seders User name and password
            smtp.EnableSsl = true;
            smtp.Send(mail);

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
    