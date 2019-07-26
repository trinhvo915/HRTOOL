﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Orient.Base.Net.Core.Api.Core.DataAccess;

namespace Orient.Base.Net.Core.Api.Migrations
{
    [DbContext(typeof(OrientNetCoreDbContext))]
    [Migration("20190624020534_UpdateDatabaseV1")]
    partial class UpdateDatabaseV1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Extension")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Attachment");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.AttachmentInJob", b =>
                {
                    b.Property<Guid>("AttachmentId");

                    b.Property<Guid>("JobId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("AttachmentId", "JobId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("AttachmentInJob");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Calendar", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CalendarTypeId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DateEnd");

                    b.Property<DateTime?>("DateStart");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("CalendarTypeId");

                    b.ToTable("Calendar");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.CalendarType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("CalendarType");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Candidate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("Address")
                        .HasMaxLength(1024);

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(512);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Facebook")
                        .HasMaxLength(512);

                    b.Property<int>("Gender");

                    b.Property<string>("LinkedIn")
                        .HasMaxLength(512);

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<string>("Twitter")
                        .HasMaxLength(512);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Candidate");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Comment", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("JobId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("UserId", "JobId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<int>("Gender");

                    b.Property<int>("LevelEmp");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmployeeInInterview", b =>
                {
                    b.Property<Guid>("EmployeeId");

                    b.Property<Guid>("InterviewId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("EmployeeId", "InterviewId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("InterviewId");

                    b.ToTable("EmployeeInInterview");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Interview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AttachmentId");

                    b.Property<Guid>("CalendarId");

                    b.Property<Guid>("CandidateId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<int>("Status");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("CalendarId");

                    b.HasIndex("CandidateId");

                    b.ToTable("Interview");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DateEnd");

                    b.Property<DateTime?>("DateStart");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<int>("Priority");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid>("ReporterId");

                    b.Property<int>("Status");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("ReporterId");

                    b.ToTable("Job");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.JobInCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("JobId");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("JobId");

                    b.ToTable("JobInCategory");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("Address")
                        .HasMaxLength(1024);

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(512);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("Email")
                        .HasMaxLength(255);

                    b.Property<string>("Facebook")
                        .HasMaxLength(512);

                    b.Property<int>("Gender");

                    b.Property<string>("Google")
                        .HasMaxLength(512);

                    b.Property<string>("LinkedIn")
                        .HasMaxLength(512);

                    b.Property<string>("Mobile")
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(512);

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<string>("ResetPasswordCode");

                    b.Property<DateTime?>("ResetPasswordExpiryDate");

                    b.Property<string>("Twitter")
                        .HasMaxLength(512);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInCalendar", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("CalendarId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("UserId", "CalendarId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("CalendarId");

                    b.ToTable("UserInCalendar");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInJob", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("JobId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("UserId", "JobId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("UserInJob");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInRole", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("Id");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("UserId", "RoleId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("UserInRole");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.AttachmentInJob", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Attachment", "Attachment")
                        .WithMany("AttachmentInJobs")
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Job", "Job")
                        .WithMany("AttachmentInJobs")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Calendar", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.CalendarType", "CalendarType")
                        .WithMany("Calendars")
                        .HasForeignKey("CalendarTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Comment", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Job", "Job")
                        .WithMany("Comments")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmployeeInInterview", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Employee", "Employee")
                        .WithMany("EmployeeInInterviews")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Interview", "Interview")
                        .WithMany("EmployeeInInterviews")
                        .HasForeignKey("InterviewId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Interview", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Attachment", "Attachment")
                        .WithMany("Interviews")
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Calendar", "Calendar")
                        .WithMany("Interviews")
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Candidate", "Candidate")
                        .WithMany("Interviews")
                        .HasForeignKey("CandidateId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.Job", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "Reporter")
                        .WithMany("Reporters")
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.JobInCategory", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Category", "Category")
                        .WithMany("JobInCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Job", "Job")
                        .WithMany("JobInCategories")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInCalendar", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Calendar", "Calendar")
                        .WithMany("UserInCalendars")
                        .HasForeignKey("CalendarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "User")
                        .WithMany("UserInCalendars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInJob", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Job", "Job")
                        .WithMany("UserInJobs")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "User")
                        .WithMany("UserInJobs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.UserInRole", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.Role", "Role")
                        .WithMany("UserInRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "User")
                        .WithMany("UserInRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
