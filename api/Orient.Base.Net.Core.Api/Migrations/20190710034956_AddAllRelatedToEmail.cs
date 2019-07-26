using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class AddAllRelatedToEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(maxLength: 512, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 512, nullable: true),
                    Host = table.Column<string>(maxLength: 512, nullable: true),
                    Port = table.Column<int>(nullable: false),
                    Username = table.Column<string>(maxLength: 512, nullable: true),
                    Password = table.Column<string>(maxLength: 512, nullable: true),
                    EnableSsl = table.Column<bool>(nullable: false),
                    UseDefaultCredentials = table.Column<bool>(nullable: false),
                    TimeOut = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Subject = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    FromName = table.Column<string>(nullable: true),
                    CC = table.Column<string>(nullable: true),
                    BCC = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailQueue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    From = table.Column<string>(maxLength: 500, nullable: true),
                    FromName = table.Column<string>(maxLength: 500, nullable: true),
                    To = table.Column<string>(maxLength: 500, nullable: true),
                    ToName = table.Column<string>(maxLength: 500, nullable: true),
                    CC = table.Column<string>(maxLength: 500, nullable: true),
                    BCC = table.Column<string>(maxLength: 500, nullable: true),
                    ReplyTo = table.Column<string>(maxLength: 500, nullable: true),
                    Subject = table.Column<string>(maxLength: 1000, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    SendLater = table.Column<DateTime>(nullable: true),
                    SentTries = table.Column<int>(nullable: false),
                    SentOn = table.Column<DateTime>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Attachments = table.Column<string>(nullable: true),
                    EmailAccountId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailQueue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailQueue_EmailAccount_EmailAccountId",
                        column: x => x.EmailAccountId,
                        principalTable: "EmailAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    Link = table.Column<string>(nullable: true),
                    EmailTemplateId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateAttachment_EmailTemplate_EmailTemplateId",
                        column: x => x.EmailTemplateId,
                        principalTable: "EmailTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailQueue_EmailAccountId",
                table: "EmailQueue",
                column: "EmailAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateAttachment_EmailTemplateId",
                table: "TemplateAttachment",
                column: "EmailTemplateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailQueue");

            migrationBuilder.DropTable(
                name: "TemplateAttachment");

            migrationBuilder.DropTable(
                name: "EmailAccount");

            migrationBuilder.DropTable(
                name: "EmailTemplate");
        }
    }
}
