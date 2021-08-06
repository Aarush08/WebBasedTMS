using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedTMS.Models;

namespace WebBasedTMS.Controllers
{
    public class TestController : Controller
    {
        private ApplicationDbContext _context;
        public TestController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ExamLogin examLogin)
        {
            var data = _context.AdmitCard.SingleOrDefault(m => m.ApplicationNo == examLogin.ApplicationNo);
            if (data == null || data.MobileNumber != examLogin.Password)
            {
                ViewBag.Message = "Invalid Credentials";
                return View();
            }
            Session["appno"] = examLogin.ApplicationNo.ToString();
            Session["Name"] = data.Name.ToString();
            Session["course"] = data.Module.ToString();
            Session["image"] = data.Image;
            return RedirectToAction("Instructions");
        }
        public ActionResult Instructions()
        {
            int data = Convert.ToInt32(Session["appno"]);
            var pdata = _context.AdmitCard.SingleOrDefault(m => m.ApplicationNo == data);
            if (pdata == null)
                return RedirectToAction("Index");
            return View(pdata);
        }
        public ActionResult Exam(int token, int? quesno)
        {
            var examtime = _context.Exams.SingleOrDefault(m => m.Activity == "Exam");
            var examscheduled = examtime.Date;
            var currenttime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            int result = DateTime.Compare(currenttime, examscheduled);
            if (result < 0)
                return RedirectToAction("Instructions");

            Session["appno"] = token.ToString();
            var sessionData = _context.SessionDatas.SingleOrDefault(m => m.ApplicationNo == token);
            if (sessionData == null)
            {
                SessionData sd = new SessionData();
                sd.ApplicationNo = token;
                sd.SessionId = token;
                sd.StartTime = DateTime.UtcNow;
                sd.EndTime = DateTime.UtcNow.AddHours(2);
                _context.SessionDatas.Add(sd);
                _context.SaveChanges();
                return Exam(token, quesno);
            }
            if (DateTime.Compare(DateTime.UtcNow, sessionData.EndTime) < 0)
            {
                var pdata = _context.AdmitCard.SingleOrDefault(m => m.ApplicationNo == token);
                ViewBag.Cname = pdata.Name;
                ViewBag.Mname = pdata.Module;
                ViewBag.ExpireTime = sessionData.EndTime;
                var data = _context.AdmitCard.SingleOrDefault(m => m.ApplicationNo == token);
                var paper = _context.TestPapers.FirstOrDefault(m => m.Module == data.Module);
                if (quesno < 1)
                    quesno = paper.Id;

                var question = _context.TestPapers.SingleOrDefault(m => m.Id == quesno);
                ViewBag.QuesNo = quesno;
                var ans = "";
                var allanswer = _context.Answers.Where(m => m.ApplicationNo == token).ToList();
                for (int i = 0; i < allanswer.Count; i++)
                {
                    if (allanswer[i].QuesNo == quesno)
                        ans = "Saved Answer:"+allanswer[i].ChoiceQue;
                }
                ViewBag.Answer = ans;
                return View(question);
            }
            return RedirectToAction("ScoreCard", new { appno = sessionData.ApplicationNo });
        }
        [HttpPost]
        public ActionResult ExamAnswers(AnswerModel answer)
        {
            var sessionData = _context.SessionDatas.First(m => m.ApplicationNo == answer.ApplicationNo);
            if (DateTime.Compare(DateTime.UtcNow, sessionData.EndTime) < 0)
            {
                _context.Answers.Add(answer);
                _context.SaveChanges();
                int nextques = (answer.QuesNo + 1);
                if (nextques >= 51)
                    nextques = 1;
                return RedirectToAction("Exam", new { token = answer.ApplicationNo, quesno = nextques});
            }
            return RedirectToAction("ScoreCard", new { appno = sessionData.ApplicationNo });
        }
        public ActionResult ScoreCard(int appno)
        {
            var score = _context.Scores.SingleOrDefault(m => m.ApplicationId == appno);
            if(score==null)
            {
                GenerateScore(appno);
                return ScoreCard(appno);
            }
            return View(score);
        }
        public void GenerateScore(int appno)
        {
            var userdata = _context.ExamParticipants.Single(m => m.Id == appno);
            var score = 0;
            var allanswers = _context.Answers.Where(m => m.ApplicationNo == appno).ToList();
            for (int i = 0; i < allanswers.Count; i++)
            {
                var queno = allanswers[i].QuesNo;
                var selchoice = allanswers[i].ChoiceQue;
                var quesdata = _context.TestPapers.Single(m=>m.Id==queno);
                var actchoice = quesdata.CorrectAnswer;
                if (selchoice.Equals(actchoice))
                    score++;
            }
            Scores scoreCard = new Scores();
            scoreCard.ApplicationId = appno;
            scoreCard.Name = userdata.Name;
            scoreCard.Module = userdata.Module;
            scoreCard.EmailId = userdata.Email;
            scoreCard.Score = score;
            if (score > 30)
                GenerateCertificate(scoreCard);
            scoreCard.DateOfExam = userdata.ExamDate;
            _context.Scores.Add(scoreCard);
            _context.SaveChanges();
        }
        public void GenerateCertificate(Scores scores)
        {
            var course = _context.Courses.Single(m => m.Module == scores.Module);
            Certificates certificates = new Certificates();
            certificates.ApplicationId = scores.ApplicationId;
            certificates.Name = scores.Name;
            certificates.Module = scores.Module;
            certificates.IssuedDate= TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            certificates.EmailId = scores.EmailId;
            certificates.Score = scores.Score;
            certificates.Instructor =course.TrainersId.ToString();
            _context.Certificates.Add(certificates);
            _context.SaveChanges();
        }

    }
}