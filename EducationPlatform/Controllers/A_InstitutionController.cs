using EducationPlatform.Auth;
using EducationPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using System.Data;

namespace EducationPlatform.Controllers
{
    public class A_InstitutionController : Controller
    {
        [InstitutionLogged]
        // GET: A_Institution
        public ActionResult Index()
        {
            return View();
        }

        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionRegistration()
        {
            return View();
        }

        [InstitutionLogged]
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

        [InstitutionLogged]
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

        public ActionResult InstitutionInformationDownload(int id)
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Create a DataTable.
            DataTable dataTable = new DataTable();
            //Add columns to the DataTable
            dataTable.Columns.Add(" ID");
            dataTable.Columns.Add(" Name");
            dataTable.Columns.Add(" Address");
            dataTable.Columns.Add(" Email");
            dataTable.Columns.Add(" Phone");
            dataTable.Columns.Add(" Password");
            dataTable.Columns.Add(" Website Link");
            dataTable.Columns.Add(" Status");

            var db = new EducationPlatformEntities();
            var ins = (from i in db.Institutions
                       where
                       i.Id == id
                       select i).SingleOrDefault();

            //Add rows to the DataTable.
            dataTable.Rows.Add(new object[] { ins.Id, ins.Name,ins.Address, ins.Email,
                ins.Phone,ins.Password,ins.WebsiteLink,ins.IsValid });
           
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new PointF(10, 10));
            // Open the document in browser after saving it
            doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            //close the document
            doc.Close(true);
            return View();
        }

        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionUpdate(int id)
        {
            var db = new EducationPlatformEntities();
            var ins = (from i in db.Institutions where 
                       i.Id == id select i).SingleOrDefault();

            return View(ins);
        }

        [InstitutionLogged]
        [HttpPost]
        public ActionResult InstitutionUpdate(Institution obj)
        {
            var db = new EducationPlatformEntities();
            var ins = (from p in db.Institutions where 
                       p.Id == obj.Id select p).SingleOrDefault();
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

        [InstitutionLogged]
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

        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionNotice()
        {
            // Session["courseID"] = id;
            return View();
        }

        [InstitutionLogged]
        [HttpPost]
        public ActionResult InstitutionNotice(Notice obj)
        {
            //var courseid = Session["courseId"].ToString();
            var db = new EducationPlatformEntities();
            var email = Session["instituteEmail"].ToString();
            //
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

        [InstitutionLogged]
        public ActionResult InstitutionSeeNotice()
        {
            var db = new EducationPlatformEntities();
            var email = Session["instituteEmail"].ToString();
            var institutionid = (from i in db.Institutions
                                 where i.Email ==email
                                 select i.Id).FirstOrDefault();
            var notice = (from i in db.Notices where i.AnnouncerId == 
                          institutionid select i).ToList();
           
            return View(notice);
        }

        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionUpdateNotice(int id)
        {
            var db = new EducationPlatformEntities();
           
            
            var notice = (from i in db.Notices
                          where i.Id == id
                          select i).FirstOrDefault();

            return View(notice);
        }

        [InstitutionLogged]
        [HttpPost]
        public ActionResult InstitutionUpdateNotice(Notice obj)
        {
            var db = new EducationPlatformEntities();
           var ins = (from p in db.Notices
                      where p.Id == obj.Id
                       select p).SingleOrDefault();
            
           //ins.CourseId = obj.CourseId;
           // ins.AnnouncedBy = obj.AnnouncedBy;
           // ins.AnnouncerId = obj.AnnouncerId;
           ins.Details = obj.Details;
           ins.Date = obj.Date;
          

            
            db.SaveChanges();
            return RedirectToAction("InstitutionSeeNotice");
        }

        [InstitutionLogged]
        public ActionResult InstitutionStudentCertificate()
        {

            var db = new EducationPlatformEntities();

            var certificaterequest = db.Certificates.ToList();
            return View(certificaterequest);
        }

        [InstitutionLogged]
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

        [InstitutionLogged]
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


        [InstitutionLogged]
        public ActionResult InstitutionStudentReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from i in db.Results where i.Id == id select i).FirstOrDefault();

            results.BackAnswer = "Yes";

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [InstitutionLogged]
        public ActionResult InstitutionStudentNotReturnResult(int id)
        {
            var db = new EducationPlatformEntities();
            var results = (from i in db.Results where i.Id == id select i).FirstOrDefault();

            results.BackAnswer = null;

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionOfferedCourse()
        {
            return View();
        }

        [InstitutionLogged]
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
                Date = obj.Date,
                MentorId = obj.MentorId,
                Photo = obj.Photo,
                InstitutionId = obj.InstitutionId,


            };
            // var course = db.Courses;

            db.Courses.Add(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [InstitutionLogged]
        public ActionResult InstitutionViewOfferedCourse()
        {
            var db = new EducationPlatformEntities();
            var email = Session["instituteEmail"].ToString();
            var institutionid = (from i in db.Institutions
                                 where i.Email == email
                                 select i.Id).FirstOrDefault();
            var course = (from i in db.Courses
                          where i.InstitutionId==
                          institutionid
                          select i).ToList();

            return View(course);
        }


        [InstitutionLogged]
        [HttpGet]
        public ActionResult InstitutionUpdateOfferedCourse(int id)
        {
            var db = new EducationPlatformEntities();


            var course = (from i in db.Courses
                          where i.Id == id
                          select i).FirstOrDefault();

            return View(course);
        }


        [InstitutionLogged]
        [HttpPost]
        public ActionResult InstitutionUpdateOfferedCourse(Cours obj)
        {
            var db = new EducationPlatformEntities();
            var ins = (from p in db.Courses
                       where p.Id == obj.Id
                       select p).SingleOrDefault();

            ins.Name = obj.Name;
            ins.InstitutionId = obj.InstitutionId;
            ins.Details = obj.Details;
            ins.Price = obj.Price;
            ins.Duration = obj.Duration;
            ins.MentorId = obj.MentorId;
            ins.Photo = obj.Photo;
            ins.Date = obj.Date;



            db.SaveChanges();
            return RedirectToAction("InstitutionViewOfferedCourse");
        }

        [InstitutionLogged]
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
   