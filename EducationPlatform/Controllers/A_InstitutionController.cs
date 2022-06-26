using EducationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EducationPlatform.Controllers
{
    public class A_InstitutionController : Controller
    {
        // GET: A_Institution
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult InstitutionRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InstitutionRegistration(Institution obj)
        {
            var db = new EducationPlatformEntities();
            var ins = new Institution()
            {
                Name = obj.Name,
                Address = obj.Address,
                Email = obj.Email,
                Phone = obj.Phone,
                Password = obj.Password,
                WebsiteLink = obj.WebsiteLink,
                IsValid = "No",
                Photo = obj.Photo,

            };
            db.Institutions.Add(ins);

            db.SaveChanges();
            return RedirectToAction("LogIn");
        }


        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Institution obj)
        {
            var db = new EducationPlatformEntities();
            var institute = (from i in db.Institutions
                             where i.Email == obj.Email
                             && i.Password == obj.Password && i.IsValid == "Yes"
                             select i).SingleOrDefault();

            if (institute != null)
            {
                Session["instituteEmail"] = obj.Email.ToString();
                //FormsAuthentication.SetAuthCookie(obj.Email, true);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.log = "Email or Password is Incorrect!";
            }

            return View();
        }


        public ActionResult InstitutionInformation()
        {
            var db = new EducationPlatformEntities();

            if (Session["instituteEmail"] != null)
            {
                var email = Session["instituteEmail"].ToString();


                var Institution = (from i in db.Institutions
                                   where
                                   i.Email == email
                                   select i).FirstOrDefault();
                return View(Institution);
            }

            return View();
        }

        [HttpGet]
        public ActionResult InstitutionUpdate(int id)
        {
            var db = new EducationPlatformEntities();
            var ins = (from i in db.Institutions where i.Id == id select i).SingleOrDefault();

            return View(ins);
        }


        [HttpPost]
        public ActionResult InstitutionUpdate(Institution obj)
        {
            var db = new EducationPlatformEntities();
            var ins = (from p in db.Institutions where p.Id == obj.Id select p).SingleOrDefault();
            ins.Id = obj.Id;
            ins.Name = obj.Name;
            ins.Address = obj.Address;
            ins.Email = obj.Email;
            ins.Phone = obj.Phone;
            ins.Password = obj.Password;
            ins.WebsiteLink = obj.WebsiteLink;
            ins.Photo = obj.Photo;

            // db.Entry(mentor).CurrentValues.SetValues(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult InstitutionDelete(Institution obj)

        {
            var db = new EducationPlatformEntities();
            var ins = (from i in db.Institutions
                       where
                       i.Id == obj.Id
                       select i).FirstOrDefault();
            db.Institutions.Remove(ins);
            db.SaveChanges();
            return RedirectToAction("LogIn");


        }

        [HttpGet]
        public ActionResult InstitutionNotice()
        {
            // Session["courseID"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult InstitutionNotice(Notice obj)
        {
            //var courseid = Session["courseId"].ToString();
            var db = new EducationPlatformEntities();
            var email = Session["instituteEmail"].ToString();
            var institutionid = (from i in db.Institutions
                                 where i.Email ==
                                 email
                                 select i.Id).FirstOrDefault();
            var institutionname = (from i in db.Institutions
                                   where i.Id ==
                                   institutionid
                                   select i.Name).FirstOrDefault();
            var notice = new Notice()
            {
                //CourseId = int.Parse(courseid),
                AnnouncedBy = institutionname,
                AnnouncerId = institutionid,
                Details = obj.Details,
                Date = obj.Date,

            };
            db.Notices.Add(notice);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult InstitutionStudentCertificate()
        {

            var db = new EducationPlatformEntities();

            var certificaterequest = db.Certificates.ToList();
            return View(certificaterequest);
        }

        public ActionResult InstitutionStudentCertificateRecommendation(int id)
        {
            var db = new EducationPlatformEntities();

            var certificateRequest = (from i in db.Certificates
                                      where i.Id == id
                                      select i).FirstOrDefault();

            certificateRequest.Status = "Pending";
            db.SaveChanges();

            return RedirectToAction("InstitutionStudentCertificate");


        }
       
        public ActionResult InstitutionStudentResult(int id)
        {
            var db = new EducationPlatformEntities();
            //var studentid = (from i in db.Transactions where i.InstitutionId == id select i.StudentId).ToList();
            //var result = (from i in db.Results where i.StudentId == studentid select i).ToString();
            var result = (from i in db.Results
                          where i.InstitutionId == id
                          select i).ToList();
            return View(result);  
        }

        public ActionResult InstitutionStudentReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from i in db.Results where i.Id == id select i).FirstOrDefault();

            results.BackAnswer = "Yes";

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InstitutionStudentNotReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from i in db.Results where i.Id == id select i).FirstOrDefault();

            results.BackAnswer = null;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]

        public ActionResult InstitutionOfferedCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InstitutionOfferedCourse(Cours obj)
        {
            var db = new EducationPlatformEntities();
            var course = new Cours()
            {
                Name = obj.Name,
                Details = obj.Details,
                Price = obj.Price,
                Duration = obj.Duration,
                Photo = obj.Photo,


            };
            // var course = db.Courses;

            db.Courses.Add(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InstitutionSalesReport()
        {
            var db = new EducationPlatformEntities();
            var email = Session["instituteEmail"].ToString();
            var id = (from i in db.Institutions
                                 where i.Email ==
                                 email
                                 select i.Id).FirstOrDefault();
            var sellingAmount = (from i in db.Transactions
                                 where (i.InstitutionId == id)
                                 select i.CreditedAmount).Sum();

            var totalSellingCourse = (from i in db.Transactions
                                      where (i.InstitutionId == id)
                                      select i.Id).Count();

            var varsityName = (from i in db.Institutions
                               where i.Id == id
                               select i.Name).FirstOrDefault();

            
            var varsityEarning = 0.4 * sellingAmount;

            ViewBag.varsityName = varsityName;
            ViewBag.totalSellingCourse = totalSellingCourse;
            ViewBag.sellingAmount = sellingAmount;
           
            ViewBag.varsityEarning = varsityEarning;

            return View();
        }

    }
}
   