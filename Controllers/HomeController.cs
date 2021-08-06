using System;
using System.Collections.Generic;
using System.IO;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedTMS.Models;

namespace WebBasedTMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult SearchCertificate()
        {
            return View();
        }
        public ActionResult Exams()
        {
            var data = _context.Exams.ToList();
            return View(data);
        }
        public ActionResult FindCertificate(int ApplicationId, string name)
        {
            var certificate = _context.Certificates.SingleOrDefault(m => m.ApplicationId == ApplicationId);
            if (certificate == null || certificate.Name != name)
                return RedirectToAction("NotFound");
            return RedirectToAction("Certificate", new { id = ApplicationId });
        }
        public ActionResult Certificate(int id)
        {
            var certificate = _context.Certificates.SingleOrDefault(m => m.ApplicationId == id);
            var data = Convert.ToInt32(certificate.Instructor);
            var trainer = _context.Trainers.SingleOrDefault(m => m.Id == data);
            ViewBag.Signature = trainer.SignName;
            ViewBag.Admin = "~/TrainersData/tms.png";
            if (certificate == null)
                return RedirectToAction("NotFound");
            return View(certificate);
        }
        public ActionResult Instructor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Instructor(HttpPostedFileBase img, HttpPostedFileBase img1, TrainerRequest trainerRequest)
        {
            string rando = Path.GetRandomFileName();
            rando = rando.Replace(".", "");
            string filename = Path.GetFileName(img.FileName);
            img.SaveAs(Server.MapPath("/TrainersData/" + rando + filename));
            trainerRequest.ImageName = "~/TrainersData/" + rando + filename;
            string filename1 = Path.GetFileName(img1.FileName);
            img.SaveAs(Server.MapPath("/TrainersData/" + rando + filename));
            trainerRequest.SignName = "~/TrainersData/" + rando + filename1;
            _context.TrainerRequests.Add(trainerRequest);
            _context.SaveChanges();
            return RedirectToAction("ThankYou");
        }
        public ActionResult ThankYou()
        {
            return View();
        }

        public ActionResult Courses()
        {
            var courses = _context.Courses.Include(c=>c.Trainers).ToList();
            return View(courses);
        }
        public ActionResult NotFound()
        {
            return View();
        }

    }
}