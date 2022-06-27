using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EducationPlatform.Auth;
using EducationPlatform.Models;

namespace EducationPlatform.Controllers
{

    [InstitutionLogged]
    public class A_StudentController : Controller
    {
        // GET: A_student
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult InstitutionStudentList()
        {
            var insemail = Session["instituteEmail"].ToString();
            var db=new EducationPlatformEntities();
            var instituteid = (from i in db.Institutions
                               where i.Email == insemail
                               select i.Id).FirstOrDefault();

            var students = (from i in db.Transactions
                            where i.InstitutionId == instituteid
                            select i).ToList();
            return View(students);
        }

    }
}