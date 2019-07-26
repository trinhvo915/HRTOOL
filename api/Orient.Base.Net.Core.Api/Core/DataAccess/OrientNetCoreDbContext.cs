using Microsoft.EntityFrameworkCore;
using Orient.Base.Net.Core.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orient.Base.Net.Core.Api.Core.DataAccess
{
	public class OrientNetCoreDbContext : DbContext
	{
		public OrientNetCoreDbContext(DbContextOptions<OrientNetCoreDbContext> options) : base(options)
		{

		}

		public DbSet<Role> Roles { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<UserInRole> UserInRoles { get; set; }

		public DbSet<Attachment> Attachments { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<Job> Jobs { get; set; }

		public DbSet<JobInCategory> JobInCategories { get; set; }

		public DbSet<UserInJob> UserInJobs { get; set; }

		public DbSet<EmailQueue> EmailQueues { get; set; }

		public DbSet<AttachmentInJob> AttachmentInJobs { get; set; }

        public DbSet<AttachmentInInterview> AttachmentInInterviews { get; set; }

        public DbSet<Calendar> Calendars { get; set; }

		public DbSet<CalendarType> CalendarTypes { get; set; }

		public DbSet<Candidate> Candidates { get; set; }

		public DbSet<UserInCalendar> UserInCalendars { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

		public DbSet<EmailAccount> EmailAccounts { get; set; }

		public DbSet<TemplateAttachment> TemplateAttachments { get; set; }

		public DbSet<TechnicalSkill> TechnicalSkills { get; set; }

        public DbSet<StepInJob> StepInJobs { get; set; }

		public DbSet<TechnicalSkillInCandidate> TechnicalSkillInCandidates { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            // Comment
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User).WithMany(u => u.Comments).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(u => u.Job).WithMany(u => u.Comments).IsRequired().OnDelete(DeleteBehavior.Restrict);

            // User In Role
            modelBuilder.Entity<UserInRole>()
                .HasOne(u => u.User).WithMany(u => u.UserInRoles).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInRole>().HasKey(t => new { t.UserId, t.RoleId });

            modelBuilder.Entity<UserInRole>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserInRole>()
                .HasOne(pt => pt.Role)
                .WithMany(p => p.UserInRoles)
                .HasForeignKey(pt => pt.RoleId);

            // User In Job
            modelBuilder.Entity<UserInJob>()
                .HasOne(u => u.User).WithMany(u => u.UserInJobs).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInJob>().HasKey(t => new { t.UserId, t.JobId });

            modelBuilder.Entity<UserInJob>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserInJobs)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserInJob>()
                .HasOne(pt => pt.Job)
                .WithMany(p => p.UserInJobs)
                .HasForeignKey(pt => pt.JobId);

            // User In Job
            modelBuilder.Entity<UserInCalendar>()
                .HasOne(u => u.User).WithMany(u => u.UserInCalendars).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserInCalendar>().HasKey(t => new { t.UserId, t.CalendarId });

            modelBuilder.Entity<UserInCalendar>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserInCalendars)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserInCalendar>()
                .HasOne(pt => pt.Calendar)
                .WithMany(p => p.UserInCalendars)
                .HasForeignKey(pt => pt.CalendarId);

            // Attachment In Job
            modelBuilder.Entity<AttachmentInJob>()
                .HasOne(u => u.Attachment).WithMany(u => u.AttachmentInJobs).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttachmentInJob>().HasKey(t => new { t.AttachmentId, t.JobId });

            modelBuilder.Entity<AttachmentInJob>()
                .HasOne(pt => pt.Attachment)
                .WithMany(p => p.AttachmentInJobs)
                .HasForeignKey(pt => pt.AttachmentId);

			modelBuilder.Entity<AttachmentInJob>()
				.HasOne(pt => pt.Job)
				.WithMany(p => p.AttachmentInJobs)
				.HasForeignKey(pt => pt.JobId);

            //Attachment In Interview
            modelBuilder.Entity<AttachmentInInterview>()
                .HasOne(u => u.Attachment).WithMany(u => u.AttachmentInInterviews).IsRequired().OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttachmentInInterview>().HasKey(t => new { t.AttachmentId, t.InterviewId });

            modelBuilder.Entity<AttachmentInInterview>()
                .HasOne(pt => pt.Attachment)
                .WithMany(p => p.AttachmentInInterviews)
                .HasForeignKey(pt => pt.AttachmentId);

            modelBuilder.Entity<AttachmentInInterview>()
                .HasOne(pt => pt.Interview)
                .WithMany(p => p.AttachmentInInterviews)
                .HasForeignKey(pt => pt.InterviewId);
		}
	}
}
