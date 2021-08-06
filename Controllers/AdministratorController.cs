using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedTMS.Models;
namespace WebBasedTMS.Controllers
{
    [Authorize]
    public class AdministratorController : Controller, IExams, ITrainer, ITestCentre, ICourse
    {
        private ApplicationDbContext _context;
        public AdministratorController()
        {
            _context = new ApplicationDbContext();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        // GET: Administrator
        public ActionResult Index()
        {
            var p1 = _context.Feedbacks.Where(c => c.Q5 == 1).Count();
            var p2 = _context.Feedbacks.Where(c => c.Q5 == 2).Count();
            var p3 = _context.Feedbacks.Where(c => c.Q5 == 3).Count();
            var p4 = _context.Feedbacks.Where(c => c.Q5 == 4).Count();
            var p5 = _context.Feedbacks.Where(c => c.Q5 == 5).Count();
            ViewBag.poor = p1 + p2;
            ViewBag.avg = p3;
            ViewBag.best = p4 + p5;
            var y2018 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2018).Count();
            var y2019 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2019).Count();
            var y2020 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2020).Count();
            ViewBag.y2018 = y2018 * 300;
            ViewBag.y2019 = y2019 * 300;
            ViewBag.y2020 = y2020 * 300;
            var e2018 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2018).Count();
            var e2019 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2019).Count();
            var e2020 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2020).Count();
            ViewBag.e2018 = y2018 * 500;
            ViewBag.e2019 = y2019 * 500;
            ViewBag.e2020 = y2020 * 500;
            var dates = _context.Exams.ToList();
            return View(dates);
        }
        public ActionResult AddExams()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddExams(Exam exam)
        {
           
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return RedirectToAction("Exams");
        }
        public ActionResult Exams()
        {
            var dates = _context.Exams.ToList();
            return View(dates);
        }
        public ActionResult RemoveExamConfirmation(int id)
        {
            var data = _context.Exams.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }
        public ActionResult RemoveExam(int id)
        {
            var data = _context.Exams.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            _context.Exams.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Exams");
        }
        public ActionResult UpdateExams(int id)
        {
            var data = _context.Exams.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateExams(Exam exam)
        {
            var dbDates = _context.Exams.Find(exam.Id);
            if (dbDates == null)
                return RedirectToAction("NotFound");
            dbDates.Activity = exam.Activity;
            dbDates.Date = exam.Date;
            dbDates.Day = exam.Day;
            _context.SaveChanges();
            return RedirectToAction("Exams");
        }

        public ActionResult RequestTrainer()
        {
            var alltrainers = _context.TrainerRequests.ToList();
            return View(alltrainers);
        }

        public ActionResult AddTrainer(int id)
        {
            var data = _context.TrainerRequests.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");

            Trainers trainer = new Trainers();

            trainer.Name = data.Name;
            trainer.Email = data.Email;
            trainer.Password = data.Password;
            trainer.MobileNumber = data.MobileNumber;
            trainer.Qualification = data.Qualification;
            trainer.Experience = data.Experience;
            trainer.Skills = data.Skills;
            trainer.SocialProfile = data.SocialProfile;
            trainer.ImageName = data.ImageName;
            trainer.SignName = data.SignName;

            _context.TrainerRequests.Remove(data);
            _context.Trainers.Add(trainer);
            _context.SaveChanges();
            SendEmail(trainer.Email, "Request Approved", "Hello " + trainer.Name + " You are approved as a trainer in our online system.Please Login and start your work.Thanks Admin");
            return RedirectToAction("Trainer");
        }

        public ActionResult Trainer()
        {
            var alltrainers = _context.Trainers.ToList();
            return View(alltrainers);
        }

        public ActionResult RemoveTrainerConfirmation(int id)
        {
            var data = _context.Trainers.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public ActionResult RemoveTrainer(int id)
        {
            var data = _context.Trainers.Find(id);
            _context.Trainers.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Trainer");
        }

        public ActionResult ViewTrainer(int id)
        {
            var data = _context.Trainers.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public ActionResult DeclineTrainer(int id)
        {
            var data = _context.TrainerRequests.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            SendEmail(data.Email, "Request Declined", "Hello " + data.Name + " You are declined as a trainer.Thanks Admin");
            _context.TrainerRequests.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("RequestTrainer");
        }

        public ActionResult ViewRequestTrainer(int id)
        {
            var data = _context.TrainerRequests.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public ActionResult DeclineTrainerConfirmation(int id)
        {
            var data = _context.TrainerRequests.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public ActionResult ApproveTrainerConfirmation(int id)
        {
            var data = _context.TrainerRequests.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public void SendEmail(string email,string subject,string body)
        {
            NotificationController notification = new NotificationController();
            notification.SendEmails(email, subject, body);
        }

        public ActionResult AddTestCentre()
        {
            TestCentres testCentre = new TestCentres();
            return View(testCentre);
        }
        [HttpPost]
        public ActionResult AddTestCentre(TestCentres testCentres)
        {
            _context.TestCentres.Add(testCentres);
            _context.SaveChanges();
            return RedirectToAction("TestCentre");
        }

        public ActionResult TestCentre()
        {
            var data = _context.TestCentres.ToList();
            return View(data);
        }

        public ActionResult RemoveTestCentreConfirmation(int id)
        {
            var data = _context.TestCentres.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");

            return View(data);
        }

        public ActionResult RemoveTestCentre(int id)
        {
            var data = _context.TestCentres.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");

            _context.TestCentres.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("TestCentre");

        }

        public ActionResult UpdateTestCentre(int id)
        {
            var data = _context.TestCentres.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");

            return View(data);
        }
        [HttpPost]
        public ActionResult UpdateTestCentre(TestCentres testCentres)
        {
            var datadb = _context.TestCentres.Find(testCentres.Id);
            if (datadb == null)
                return RedirectToAction("NotFound");

            datadb.Name = testCentres.Name;
            datadb.Address = testCentres.Address;
            datadb.City = testCentres.City;
            datadb.State = testCentres.State;
            datadb.PinCode = testCentres.PinCode;
            datadb.Email = testCentres.Email;
            datadb.ContactNo = testCentres.ContactNo;
            datadb.TotalSeats = testCentres.TotalSeats;
            datadb.AvailableSeats = testCentres.AvailableSeats;

            _context.SaveChanges();
            return RedirectToAction("TestCentre");
        }

        public ActionResult ViewFeedbacks()
        {
            var data = _context.Feedbacks.ToList();
            return View(data);
        }

        public ActionResult ViewScores()
        {
            var data = _context.Scores.ToList();
            return View(data);
        }

        public ActionResult ViewExamParticipants()
        {
            var data = _context.ExamParticipants.ToList();
            return View(data);
        }

        public ActionResult AddCourses()
        {
            ViewBag.Trainers = new SelectList(_context.Trainers, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult AddCourses(Courses courses)
        {
            courses.Content = "Updated Soon...!!!";
            _context.Courses.Add(courses);
            LiveStreaming liveStreaming = new LiveStreaming();
            liveStreaming.Module = courses.Module;
            liveStreaming.MeetingId = "Updated Soon...!!!";
            _context.LiveStreamings.Add(liveStreaming);
            _context.SaveChanges();
            return RedirectToAction("Courses");
        }

        public ActionResult Courses()
        {
            var courses = _context.Courses.Include(c => c.Trainers).ToList();
            return View(courses);
        }

        public ActionResult RemoveCoursesConfirmation(int id)
        {
            var data = _context.Courses.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            return View(data);
        }

        public ActionResult RemoveCourses(int id)
        {
            var data = _context.Courses.Find(id);
            if (data == null)
                return RedirectToAction("NotFound");
            _context.Courses.Remove(data);
            _context.SaveChanges();
            return RedirectToAction("Courses");
        }

        public ActionResult SendEmails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendEmails(SendEmails sendEmails)
        {
            if(sendEmails.ToWhom=="FA")
            {
                var data = _context.Trainers.ToList();
                foreach (var trainers in data)
                {
                    SendEmail(trainers.Email, sendEmails.Subject, sendEmails.Body);
                }
            }
            else if (sendEmails.ToWhom == "CA")
            {
                var data = _context.Users.ToList();
                foreach (var candidate in data)
                {
                    SendEmail(candidate.Email, sendEmails.Subject, sendEmails.Body);
                }
            }
            else if (sendEmails.ToWhom == "TC")
            {
                var data = _context.TestCentres.ToList();
                foreach (var tc in data)
                {
                    SendEmail(tc.Email, sendEmails.Subject, sendEmails.Body);
                }
            }
            else
            {
                SendEmail("himanshupraveengoyal@gmail.com", sendEmails.Subject, sendEmails.Body);
            }
            return View();
        }

        public ActionResult AdminProfile()
        {
            var email = User.Identity.Name;
            var data = _context.Users.SingleOrDefault(m => m.Email == email);
            return View(data);
        }
    }
}