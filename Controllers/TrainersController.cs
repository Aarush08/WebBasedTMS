using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedTMS.Models;
using Excel=Microsoft.Office.Interop.Excel;

namespace WebBasedTMS.Controllers
{
    [Authorize]
    public class TrainersController : Controller
    {
        // GET: Trainers
        private ApplicationDbContext _context;
        public TrainersController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var module = GetModule();
            var alldata = _context.Feedbacks.Where(m => m.Module == module).ToList();
            var p1 = 0;var p2 = 0;var p3 = 0;var p4 = 0; var p5 = 0;
            foreach (var data in alldata)
            {
                if (data.Q5 == 1) p1++;
                if (data.Q5 == 2) p2++;
                if (data.Q5 == 3) p3++;
                if (data.Q5 == 4) p4++;
                if (data.Q5 == 5) p5++;
            }
            ViewBag.poor = p1 + p2;
            ViewBag.avg = p3;
            ViewBag.best = p4 + p5;
            var y2018 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2018).Count();
            var y2019 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2019).Count();
            var y2020 = _context.CourseParticipants.Where(c => c.RegTime.Year == 2020).Count();
            ViewBag.y2018 = y2018 * 200;
            ViewBag.y2019 = y2019 * 200;
            ViewBag.y2020 = y2020 * 200;
            var e2018 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2018).Count();
            var e2019 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2019).Count();
            var e2020 = _context.ExamParticipants.Where(c => c.ExamDate.Year == 2020).Count();
            ViewBag.e2018 = y2018 * 350;
            ViewBag.e2019 = y2019 * 350;
            ViewBag.e2020 = y2020 * 350;

            return View();
        }
        public ActionResult Scores()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var data = _context.Scores.Where(m=>m.Module==module).ToList();
            return View(data);
        }
        public ActionResult ViewFeedbacks()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var data = _context.Feedbacks.Where(m => m.Module == module).ToList();
            return View(data);
        }
        public ActionResult CourseContent()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var courses = _context.Courses.SingleOrDefault(m=>m.Module==module);
            return View(courses);
        }
        [HttpPost]
        public ActionResult CourseContent(Courses courses)
        {
            var dataindb = _context.Courses.SingleOrDefault(m => m.Id == courses.Id);
            dataindb.Content = courses.Content;
            _context.SaveChanges();
            return View(courses);
        }
        public ActionResult ViewDoubts()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var doubt = _context.Doubts.Where(m => m.Module == module).ToList();
            return View(doubt);
        }
        public ActionResult QuestionDetails(int id)
        {
            var ques = _context.Doubts.SingleOrDefault(m => m.Id == id);
            if (ques == null)
                return HttpNotFound();
            return View(ques);
        }
        [HttpPost]
        public ActionResult QuestionDetails(Doubt doubt)
        {
            var ques = _context.Doubts.Single(m => m.Id == doubt.Id);
            ques.Answer = doubt.Answer;
            ques.Status = true;
            _context.SaveChanges();
            return RedirectToAction("ViewDoubts");
        }
        public ActionResult LiveStreaming()
        {
            string module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            LiveStreaming streaming = new LiveStreaming();
            streaming.Module = module;
            return View(streaming);
        }
        [HttpPost]
        public ActionResult LiveStreaming(LiveStreaming streaming)
        {
            string module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var data = _context.LiveStreamings.Where(m => m.Module == module).ToList();
            if (data==null)
                return RedirectToAction("NothingFound");
            data[0].MeetingId = streaming.MeetingId;
            _context.SaveChanges();
            return RedirectToAction("LiveStreaming");
        }
        public string GetModule()
        {
            var email = User.Identity.Name;
            var data = _context.Trainers.FirstOrDefault(m => m.Email == email);
            if (data == null)
                return "";
            var course = _context.Courses.FirstOrDefault(m => m.TrainersId == data.Id);
            if (course == null)
                return "";
            return course.Module;
        }
        public ActionResult UploadTestPaper()
        {
            var module=GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");

            ViewBag.Module = module;
            return View();
        }
        [HttpPost]
        public ActionResult UploadTestPaper(HttpPostedFileBase excelfile)
        {
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string Module = GetModule();
                string path = Server.MapPath("~/Excel/" + excelfile.FileName);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                excelfile.SaveAs(path);
                //Reading File
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Open(path);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                Excel.Range range = worksheet.UsedRange;
                List<TestPapers> papers = new List<TestPapers>();
                for (int row = 2; row <= range.Rows.Count; row++)
                {
                    TestPapers p = new TestPapers();
                    p.Question = ((Excel.Range)range.Cells[row, 2]).Text;
                    p.Choice1 = ((Excel.Range)range.Cells[row, 3]).Text;
                    p.Choice2 = ((Excel.Range)range.Cells[row, 4]).Text;
                    p.Choice3 = ((Excel.Range)range.Cells[row, 5]).Text;
                    p.Choice4 = ((Excel.Range)range.Cells[row, 6]).Text;
                    p.CorrectAnswer = ((Excel.Range)range.Cells[row, 7]).Text;
                    p.Module = Module;
                    papers.Add(p);
                    _context.TestPapers.Add(p);
                }
                _context.SaveChanges();
                application.Workbooks.Close();
                //File Reading Ends
                return RedirectToAction("TestPaper");
            }
            return RedirectToAction("UploadTestPaper");
        }
        public ActionResult TestPaper()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var testPapers = _context.TestPapers.Where(m => m.Module == module).ToList();
            return View(testPapers);
        }

        public ActionResult UploadResources()
        {
            ViewBag.Module = GetModule();
            return View();
        }
        [HttpPost]
        public ActionResult UploadResources(HttpPostedFileBase uploadedfile, Rescources rescources)
        {
            string rando = Path.GetRandomFileName();
            rando = rando.Replace(".", "");
            string filename = Path.GetFileName(uploadedfile.FileName);
            uploadedfile.SaveAs(Server.MapPath("/Resources/" + rando + filename));
            rescources.FileName = "~/Resources/" + rando + filename;
            rescources.Module = GetModule();
            _context.Rescources.Add(rescources);
            _context.SaveChanges();
            return RedirectToAction("Resources");
        }

        public ActionResult Resources()
        {
            var module = GetModule();
            if (module == "")
                return RedirectToAction("NothingFound");
            var data = _context.Rescources.Where(m => m.Module == module).ToList();
            return View(data);
        }

        public ActionResult NothingFound()
        {
            return View();
        }

        public ActionResult TrainerProfile()
        {
            var email = User.Identity.Name;
            var data = _context.Trainers.SingleOrDefault(m => m.Email == email);
            return View(data);
        }
    }
}