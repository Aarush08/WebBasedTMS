using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebBasedTMS.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(13)]
        public string MobileNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int Role { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Trainers> Trainers { get; set; }
        public DbSet<TrainerRequest> TrainerRequests { get; set; }
        public DbSet<TestCentres> TestCentres { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<ExamParticipants> ExamParticipants { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<AdmitCard> AdmitCard { get; set; }
        public DbSet<TestPapers> TestPapers { get; set; }
        public DbSet<AnswerModel> Answers { get; set; }
        public DbSet<SessionData> SessionDatas { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        public DbSet<Doubt> Doubts { get; set; }
        public DbSet<Rescources> Rescources { get; set; }
        public DbSet<CourseParticipants> CourseParticipants { get; set; }
        public DbSet<LiveStreaming> LiveStreamings { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}