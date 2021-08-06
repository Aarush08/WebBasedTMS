using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBasedTMS.Models;
using Stripe.Infrastructure;

namespace WebBasedTMS.Controllers
{
    public class ExamController : Controller
    {
        private ApplicationDbContext _context;
        public ExamController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var dates = _context.Exams.ToList();
            return View(dates);
        }
        public ActionResult Register()
        {
            var edate = _context.Exams.SingleOrDefault(m => m.Activity == "Exam");
            if (edate == null)
                return RedirectToAction("Index");
            ViewBag.TestCentre = new SelectList(_context.TestCentres, "Id", "City");
            ViewBag.Module = new SelectList(_context.Courses, "Id", "Module");
            return View();
        }
        [HttpPost]
        public ActionResult Register(HttpPostedFileBase Image, ExamParticipants examParticipants)
        {
            if(Image!=null)
            {
                int id = int.Parse(examParticipants.Module);
                var data = _context.Courses.Find(id);
                examParticipants.Module = data.Module;
                int tid = int.Parse(examParticipants.ExamCity);
                var data1 = _context.TestCentres.Find(tid);
                examParticipants.ExamCity = data1.City;
                var edate=_context.Exams.SingleOrDefault(m=>m.Activity=="Exam");
                examParticipants.ExamDate = edate.Date;
                string rando = Path.GetRandomFileName();
                rando = rando.Replace(".", "");
                string filename = Path.GetFileName(Image.FileName);
                Image.SaveAs(Server.MapPath("/ExamCandidate/"+ rando + filename));
                examParticipants.Image = "/ExamCandidate/" + rando + filename;
                _context.ExamParticipants.Add(examParticipants);
                _context.SaveChanges();
                var gid = examParticipants.Id;
                var msg = "Hello " + examParticipants.Name + "your application number "+ gid +" thank you for Registering for certificate exam your details is saved make the payment to complete registeration and get Admit Cards.Regards,Admin";
                SendEmail(examParticipants.Email, "Exam Registration",msg);
                return RedirectToAction("ViewApplication", new { id = gid });
            }
            return View();
        }
        public ActionResult SearchApplication()
        {
            var edate = _context.Exams.SingleOrDefault(m => m.Activity == "Exam");
            if (edate == null)
                return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public ActionResult SearchApplication(FindParticipants findParticipants)
        {
            var data = _context.ExamParticipants.SingleOrDefault(m => m.Id == findParticipants.AppNo);
            if (data == null || data.Name != findParticipants.Name)
                return RedirectToAction("NotFound");
            return RedirectToAction("ViewApplication/"+data.Id);
        }
        public ActionResult ViewApplication(int id)
        {
            var data = _context.ExamParticipants.SingleOrDefault(m => m.Id == id);
            if (data == null)
                return RedirectToAction("SearchApplication");
            return View(data);
        }
        public void SendEmail(string email,string subject,string body)
        {
            NotificationController notification = new NotificationController();
            notification.SendEmails(email, subject, body);
        }
        public ActionResult MakePayment(int id)
        {
            var data = _context.ExamParticipants.Find(id);
            Payment payment = new Payment();
            payment.Id = id;
            payment.Name = data.Name;
            return View(payment);
        }
        [HttpPost]
        [Obsolete]
        public ActionResult MakePayment(Payment payment)
        {
            var data = _context.ExamParticipants.Find(payment.Id);
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
            myCustomer.Email = data.Email;
            myCustomer.Name = data.Name;
            var customerService = new Stripe.CustomerService();
            Stripe.Customer stripeCustomer = customerService.Create(myCustomer);

            var options = new Stripe.ChargeCreateOptions
            {
                Amount = 50000,
                Currency = "INR",
                ReceiptEmail = data.Email,
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
                return RedirectToAction("ViewApplication", new { id = payment.Id });
        }
        public ActionResult PaymentSuccess(int id,string payid)
        {
            var data = _context.ExamParticipants.Find(id);
            data.PaymmentStatus = true;
            data.PaymentId = payid;
            GenerateAdmitCard(id);
            _context.SaveChanges();
            return RedirectToAction("AdmitCards", new { id = id });
        }
        public void GenerateAdmitCard(int appno)
        {
            var pdata = _context.ExamParticipants.SingleOrDefault(m => m.Id == appno);
            var tdata = _context.TestCentres.Single(m => m.City == pdata.ExamCity);
            AdmitCard admitCard = new AdmitCard();
            admitCard.ApplicationNo = pdata.Id;
            admitCard.Name = pdata.Name;
            admitCard.MobileNumber = pdata.MobileNumber;
            admitCard.Image = pdata.Image;
            admitCard.DateOfExam = pdata.ExamDate;
            admitCard.ExamCentre = tdata.Name +" , "+ tdata.Address + " , " + tdata.City + " , " + tdata.State + " , " + tdata.PinCode;
            admitCard.Module = pdata.Module;
            _context.AdmitCard.Add(admitCard);
            _context.SaveChanges();
        }
        public ActionResult AdmitCards(int id)
        {
            var data = _context.AdmitCard.Single(m => m.ApplicationNo == id);
            if(data==null)
            {
                GenerateAdmitCard(id);
                return AdmitCards(id);
            }
            return View(data);
        }
        public ActionResult SearchCertificate()
        {
            return View();
        }
        public ActionResult FindCertificate(int ApplicationId, string name)
        {
            var certificate = _context.Certificates.SingleOrDefault(m => m.ApplicationId == ApplicationId);
            if (certificate == null || certificate.Name != name)
                return RedirectToAction("NotFound");
            return RedirectToAction("Certificate"  , new { id =ApplicationId });
        }
        public ActionResult Certificate(int id)
        {
            var certificate = _context.Certificates.SingleOrDefault(m => m.ApplicationId == id);
            var data = Convert.ToInt32(certificate.Instructor);
            var trainer = _context.Trainers.SingleOrDefault(m => m.Id ==data);
            ViewBag.Signature = trainer.SignName;
            ViewBag.Admin = "~/TrainersData/tms.png";
            if (certificate == null)
                return RedirectToAction("NotFound");
            return View(certificate);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}