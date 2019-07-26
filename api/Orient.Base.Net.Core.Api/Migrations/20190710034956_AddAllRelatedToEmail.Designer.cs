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
    [Migration("20190710034956_AddAllRelatedToEmail")]
    partial class AddAllRelatedToEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

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

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmailAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(512);

                    b.Property<string>("Email")
                        .HasMaxLength(512);

                    b.Property<bool>("EnableSsl");

                    b.Property<string>("Host")
                        .HasMaxLength(512);

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Password")
                        .HasMaxLength(512);

                    b.Property<int>("Port");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<int>("TimeOut");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.Property<bool>("UseDefaultCredentials");

                    b.Property<string>("Username")
                        .HasMaxLength(512);

                    b.HasKey("Id");

                    b.ToTable("EmailAccount");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmailQueue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Attachments");

                    b.Property<string>("BCC")
                        .HasMaxLength(500);

                    b.Property<string>("Body");

                    b.Property<string>("CC")
                        .HasMaxLength(500);

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid?>("EmailAccountId");

                    b.Property<string>("From")
                        .HasMaxLength(500);

                    b.Property<string>("FromName")
                        .HasMaxLength(500);

                    b.Property<string>("Message");

                    b.Property<int>("Priority");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<string>("ReplyTo")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("SendLater");

                    b.Property<DateTime?>("SentOn");

                    b.Property<int>("SentTries");

                    b.Property<string>("Subject")
                        .HasMaxLength(1000);

                    b.Property<string>("To")
                        .HasMaxLength(500);

                    b.Property<string>("ToName")
                        .HasMaxLength(500);

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("EmailAccountId");

                    b.ToTable("EmailQueue");
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmailTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BCC");

                    b.Property<string>("Body");

                    b.Property<string>("CC");

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<string>("From");

                    b.Property<string>("FromName");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<string>("Subject");

                    b.Property<int>("Type");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("EmailTemplate");
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

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.TemplateAttachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedBy");

                    b.Property<DateTime?>("CreatedOn");

                    b.Property<Guid?>("DeletedBy");

                    b.Property<DateTime?>("DeletedOn");

                    b.Property<Guid>("EmailTemplateId");

                    b.Property<string>("FileName");

                    b.Property<string>("Link");

                    b.Property<bool>("RecordActive");

                    b.Property<bool>("RecordDeleted");

                    b.Property<int>("RecordOrder");

                    b.Property<Guid?>("UpdatedBy");

                    b.Property<DateTime?>("UpdatedOn");

                    b.HasKey("Id");

                    b.HasIndex("EmailTemplateId");

                    b.ToTable("TemplateAttachment");
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
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.EmailQueue", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.EmailAccount")
                        .WithMany("EmailQueues")
                        .HasForeignKey("EmailAccountId");
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

            modelBuilder.Entity("Orient.Base.Net.Core.Api.Core.Entities.TemplateAttachment", b =>
                {
                    b.HasOne("Orient.Base.Net.Core.Api.Core.Entities.EmailTemplate", "EmailTemplate")
                        .WithMany("TemplateAttachments")
                        .HasForeignKey("EmailTemplateId")
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
