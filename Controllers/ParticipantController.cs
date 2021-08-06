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
    public class ParticipantController : Controller
    {
        // GET: Participant
        private ApplicationDbContext _context;
        public ParticipantController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var email = User.Identity.Name;
            var p1 = _context.Feedbacks.Where(c => c.Q5 == 1).Count();
            var p2 = _context.Feedbacks.Where(c => c.Q5 == 2).Count();
            var p3 = _context.Feedbacks.Where(c => c.Q5 == 3).Count();
            var p4 = _context.Feedbacks.Where(c => c.Q5 == 4).Count();
            var p5 = _context.Feedbacks.Where(c => c.Q5 == 5).Count();
            ViewBag.poor = p1 + p2;
            ViewBag.avg = p3;
            ViewBag.best = p4 + p5;
            var allcourses = _context.CourseParticipants.Where(m => m.EmailAddress == email).ToList();
            var y2018 =0; var y2019 =0; var y2020 =0;
            foreach (var courses in allcourses)
            {
                if (courses.StartTime.Year == 2018) y2018++;
                if (courses.StartTime.Year == 2019) y2019++;
                if (courses.StartTime.Year == 2020) y2020++;
            }
            ViewBag.y2018 = y2018;
            ViewBag.y2019 = y2019;
            ViewBag.y2020 = y2020;
            return View();
        }

        //Display All Course Content
        public ActionResult Contents()
        {
            var course = _context.Courses.Include(c => c.Trainers).ToList();
            return View(course);
        }
        public ActionResult CourseVideos(string course)
        {
            var resource = _context.Rescources.Where(m => m.Module == course).SingleOrDefault(m => m.ResourceType == "Introductory Video");
            return View(resource);
        }
        public ActionResult Registration()
        {
            ViewBag.Module = new SelectList(_context.Courses, "Id", "Module");
            ViewBag.Name = FindName();
            ViewBag.MobileNumber = FindMob();
            return View();
        }
        [HttpPost]
        public ActionResult Registration(CourseParticipants courseParticipants)
        {
            var id = Convert.ToInt32(courseParticipants.Module);
            var mod = _context.Courses.Single(m => m.Id == id);
            var name = FindName();
            var mobile = FindMob();
            var email = User.Identity.Name;
            var currenttime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            courseParticipants.Module = mod.Module;
            courseParticipants.Name = name;
            courseParticipants.MobileNumber = mobile;
            courseParticipants.EmailAddress = email;
            courseParticipants.RegTime = currenttime;
            courseParticipants.StartTime = mod.StartDate;
            _context.CourseParticipants.Add(courseParticipants);
            _context.SaveChanges();
            return RedirectToAction("RegisteredCourses");
        }
        public ActionResult LiveStreaming()
        {
            var email = User.Identity.Name;
            var allcourse = _context.CourseParticipants.Where(m => m.EmailAddress == email).ToList();
            var today = DateTime.UtcNow;
            var list = new List<LiveStreaming>();
            foreach (var course in allcourse)
            {
                var nextten = course.StartTime.AddDays(10);
                if (DateTime.Compare(today, nextten) <= 0)
                {
                    var data = _context.LiveStreamings.SingleOrDefault(m => m.Module == course.Module);
                    list.Add(data);
                }
            }
            if(list.Count==0)
                return RedirectToAction("RegisteredCourses");
            return View(list);
        }
        public ActionResult RegisteredCourses()
        {
            var email = User.Identity.Name;
            var data = _context.CourseParticipants.Where(m => m.EmailAddress == email).ToList();
            return View(data);
        }
        public ActionResult MakePayment(int id)
        {
            var data = _context.CourseParticipants.Find(id);
            Payment payment = new Payment();
            payment.Id = id;
            payment.Name = data.Name;
            return View(payment);
        }
        [HttpPost]
        [Obsolete]
        public ActionResult MakePayment(Payment payment)
        {
            var data = _context.CourseParticipants.SingleOrDefault(m=>m.Id==payment.Id);
            Stripe.StripeConfiguration.SetApiKey("sk_test_1TBDARH9II7vAtqwZzEXznZS00KVo6vSEd");
            Stripe.CreditCardOptions card = new Stripe.CreditCardOptions();
            card.Name = payment.CardHolderName;
            card.Number = payment.CardNumber;
            card.ExpYear = payment.ExpiryYear;
            card.ExpMonth = payment.ExpiryMonth;
            card.Cvc = payment.Cvv.ToString();
            //Assign Card to Token Object and create Token  
            Stripe.TokenCreateOptions token = new Stripe.TokenCreateOptions();
            token.Card = card;
            Stripe.TokenService serviceToken = new Stripe.TokenService();
            Stripe.Token newToken = serviceToken.Create(token);
            Stripe.CustomerCreateOptions myCustomer = new Stripe.CustomerCreateOptions();
            myCustomer.Email = data.EmailAddress;
            myCustomer.Name = data.Name;
            var customerService = new Stripe.CustomerService();
            Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

            var options = new Stripe.ChargeCreateOptions
            {
                Amount = 35000,
                Currency = "INR",
                ReceiptEmail = data.EmailAddress,
                Source = "tok_visa",
                Description = payment.Reason

            };
            //and Create Method of this object is doing the payment execution.  
            var service = new Stripe.ChargeService();
            Stripe.Charge charge = service.Create(options); // This will do the Payment 
            var paystatus = charge.Status;
            var paymentid = charge.Id;
            if (paystatus == "succeeded")
                return RedirectToAction("PaymentSuccess", new { id = payment.Id, payid = paymentid });
            else
                return RedirectToAction("RegisteredCourses");
        }

        public ActionResult PaymentSuccess(int id,string payid)
        {
            var data = _context.CourseParticipants.Single(m => m.Id == id);
            data.PaymentStatus = true;
            data.PaymentId = payid;
            _context.SaveChanges();
            return RedirectToAction("RegisteredCourses");
        }
        public ActionResult Feedback()
        {
            var email = User.Identity.Name;
            var allcourse = _context.CourseParticipants.Where(c => c.EmailAddress == email).ToList();
            foreach (var courses in allcourse)
            {
                bool coursestats = CourseStatus(courses.Id);
                if (coursestats == true)
                {
                    ViewBag.Module = courses.Module;
                    return View();
                }
            }
            return RedirectToAction("PurchaseCourse");
        }
        [HttpPost]
        public ActionResult Feedback(Feedbacks feedbacks)
        {
            var course = _context.Courses.SingleOrDefault(m => m.Module == feedbacks.Module);
            var trainer = _context.Trainers.SingleOrDefault(m => m.Id == course.TrainersId);
            feedbacks.Faculty = trainer.Name;
            _context.Feedbacks.Add(feedbacks);
            _context.SaveChanges();
            return RedirectToAction("Contents");
        }

        public ActionResult AskDoubts()
        {
            var email = User.Identity.Name;
            var allcourse = _context.CourseParticipants.Where(c => c.EmailAddress == email).ToList();
            foreach (var courses in allcourse)
            {
                bool coursestats = CourseStatus(courses.Id);
                if (coursestats == true)
                {
                    ViewBag.Module = courses.Module;
                    return View();
                }
            }
            return RedirectToAction("PurchaseCourse");
        }
        [HttpPost]
        public ActionResult AskDoubts(Doubt doubt)
        {
            var email = User.Identity.Name;
            doubt.EmailId = email;
            var currenttime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            doubt.TimeStamp = currenttime;
            _context.Doubts.Add(doubt);
            _context.SaveChanges();
            return RedirectToAction("Doubt");
        }

        public ActionResult Doubt()
        {
            var email = User.Identity.Name;
            var allcourse = _context.CourseParticipants.Where(c => c.EmailAddress == email).ToList();
            string module="";
            foreach (var courses in allcourse)
            {
                bool coursestats = CourseStatus(courses.Id);
                if (coursestats == true)
                {
                    module = courses.Module;
                }
            }
            if(module=="")
                return RedirectToAction("PurchaseCourse");
            var doubt = _context.Doubts.Where(m => m.Module == module);
            return View(doubt);
        }
        public ActionResult ViewAnswer(int id)
        {
            var ques = _context.Doubts.Find(id);
            return View(ques);
        }

        public ActionResult Resources()
        {
            var email = User.Identity.Name;
            var allcourse = _context.CourseParticipants.Where(c => c.EmailAddress == email).ToList();
            foreach (var courses in allcourse)
            {
                bool coursestats = CourseStatus(courses.Id);
                if (coursestats == true)
                {
                    var module = courses.Module;
                    var resources = _context.Rescources.Where(m => m.Module == module).ToList();
                    return View(resources);
                }
            }
            return RedirectToAction("PurchaseCourse");
        }
        public ActionResult PurchaseCourse()
        {
            return View();
        }
        public bool CourseStatus(int id)
        {
            var participant = _context.CourseParticipants.Single(m => m.Id == id);
            if (participant.PaymentStatus == false)
                return false;
            else
            {
                var today = DateTime.UtcNow;
                var nextten = participant.StartTime.AddDays(10);
                if (DateTime.Compare(today, nextten) > 0)
                    return false;
                else
                    return true;
            }
        }
        public string FindName()
        {
            var email = User.Identity.Name;
            var data = _context.Users.FirstOrDefault(m => m.Email == email);
            return data.Name;
        }
        public string FindMob()
        {
            var email = User.Identity.Name;
            var data = _context.Users.FirstOrDefault(m => m.Email == email);
            return data.MobileNumber;
        }

        public ActionResult ParticipantProfile()
        {
            var email = User.Identity.Name;
            var data = _context.Users.SingleOrDefault(m => m.Email == email);
            return View(data);
        }
    }
}