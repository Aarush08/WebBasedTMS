using System.Web.Mvc;
using WebBasedTMS.Models;

namespace WebBasedTMS.Controllers
{
    interface IExams
    {
        ActionResult AddExams();
        ActionResult AddExams(Exam exam);
        ActionResult Exams();
        ActionResult RemoveExamConfirmation(int id);
        ActionResult RemoveExam(int id);
        ActionResult UpdateExams(int id);
        ActionResult UpdateExams(Exam exam);
    }
    interface ITrainer
    {
        ActionResult RequestTrainer();
        ActionResult ViewRequestTrainer(int id);
        ActionResult DeclineTrainer(int id);
        ActionResult DeclineTrainerConfirmation(int id);
        ActionResult ApproveTrainerConfirmation(int id);
        ActionResult AddTrainer(int id);
        ActionResult Trainer();
        ActionResult RemoveTrainerConfirmation(int id);
        ActionResult RemoveTrainer(int id);
        ActionResult ViewTrainer(int id);
    }

    interface ITestCentre
    {
        ActionResult AddTestCentre();
        ActionResult AddTestCentre(TestCentres testCentres);
        ActionResult TestCentre();
        ActionResult RemoveTestCentreConfirmation(int id);
        ActionResult RemoveTestCentre(int id);
        ActionResult UpdateTestCentre(int id);
        ActionResult UpdateTestCentre(TestCentres testCentres);
    }

    interface ICourse
    {
        ActionResult AddCourses();
        ActionResult AddCourses(Courses courses);
        ActionResult Courses();
        ActionResult RemoveCoursesConfirmation(int id);
        ActionResult RemoveCourses(int id);
    }

}