namespace WebBasedTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAllTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdmitCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationNo = c.Int(nullable: false),
                        Name = c.String(),
                        MobileNumber = c.String(),
                        Image = c.String(),
                        DateOfExam = c.DateTime(nullable: false),
                        ExamCentre = c.String(),
                        Module = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AnswerModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationNo = c.Int(nullable: false),
                        QuesNo = c.Int(nullable: false),
                        ChoiceQue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Certificates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Module = c.String(),
                        Instructor = c.String(),
                        EmailId = c.String(),
                        IssuedDate = c.DateTime(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CourseParticipants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegTime = c.DateTime(nullable: false),
                        Name = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        EmailAddress = c.String(nullable: false),
                        Module = c.String(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        PaymentStatus = c.Boolean(nullable: false),
                        PaymentId = c.String(),
                    })
                .PrimaryKey(t => t.Id);


            CreateTable(
    "dbo.Trainers",
    c => new
    {
        Id = c.Int(nullable: false, identity: true),
        Name = c.String(nullable: false),
        Email = c.String(nullable: false),
        Password = c.String(nullable: false),
        MobileNumber = c.String(nullable: false),
        Qualification = c.String(nullable: false),
        Experience = c.Int(nullable: false),
        Skills = c.String(nullable: false),
        SocialProfile = c.String(nullable: false),
        ImageName = c.String(nullable: false),
        SignName = c.String(nullable: false),
    })
    .PrimaryKey(t => t.Id);


            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Duration = c.Int(nullable: false),
                        Content = c.String(),
                        TrainersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trainers", t => t.TrainersId, cascadeDelete: true)
                .Index(t => t.TrainersId);
            

            
            CreateTable(
                "dbo.Doubts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailId = c.String(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        Module = c.String(nullable: false),
                        Question = c.String(nullable: false),
                        Answer = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExamParticipants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        DetailsConfirm = c.Boolean(nullable: false),
                        DateofBirth = c.DateTime(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PinCode = c.Int(nullable: false),
                        Image = c.String(nullable: false),
                        Module = c.String(nullable: false),
                        ExamDate = c.DateTime(nullable: false),
                        ExamCity = c.String(nullable: false),
                        PaymmentStatus = c.Boolean(nullable: false),
                        PaymentId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Activity = c.String(nullable: false),
                        Day = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module = c.String(nullable: false),
                        Faculty = c.String(nullable: false),
                        Q1 = c.Int(nullable: false),
                        Q2 = c.Int(nullable: false),
                        Q3 = c.Int(nullable: false),
                        Q4 = c.Int(nullable: false),
                        Q5 = c.Int(nullable: false),
                        Suggestions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LiveStreamings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module = c.String(nullable: false),
                        MeetingId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rescources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module = c.String(nullable: false),
                        ResourceType = c.String(nullable: false),
                        FileName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Scores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationId = c.Int(nullable: false),
                        DateOfExam = c.DateTime(nullable: false),
                        Module = c.String(),
                        Name = c.String(),
                        EmailId = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SessionDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationNo = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestCentres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        PinCode = c.Int(nullable: false),
                        Email = c.String(nullable: false),
                        ContactNo = c.String(nullable: false),
                        TotalSeats = c.Int(nullable: false),
                        AvailableSeats = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestPapers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Module = c.String(),
                        Question = c.String(),
                        Choice1 = c.String(),
                        Choice2 = c.String(),
                        Choice3 = c.String(),
                        Choice4 = c.String(),
                        CorrectAnswer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainerRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        MobileNumber = c.String(nullable: false),
                        Qualification = c.String(nullable: false),
                        Experience = c.Int(nullable: false),
                        Skills = c.String(nullable: false),
                        SocialProfile = c.String(nullable: false),
                        ImageName = c.String(nullable: false),
                        SignName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        MobileNumber = c.String(nullable: false, maxLength: 13),
                        Name = c.String(nullable: false, maxLength: 50),
                        Role = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Courses", "TrainersId", "dbo.Trainers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Courses", new[] { "TrainersId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TrainerRequests");
            DropTable("dbo.TestPapers");
            DropTable("dbo.TestCentres");
            DropTable("dbo.SessionDatas");
            DropTable("dbo.Scores");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Rescources");
            DropTable("dbo.LiveStreamings");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Exams");
            DropTable("dbo.ExamParticipants");
            DropTable("dbo.Doubts");
            DropTable("dbo.Trainers");
            DropTable("dbo.Courses");
            DropTable("dbo.CourseParticipants");
            DropTable("dbo.Certificates");
            DropTable("dbo.AnswerModels");
            DropTable("dbo.AdmitCards");
        }
    }
}
