using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class AddAttachmentInInterview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interview_Attachment_AttachmentId",
                table: "Interview");

            migrationBuilder.DropIndex(
                name: "IX_Interview_AttachmentId",
                table: "Interview");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Interview");

            migrationBuilder.CreateTable(
                name: "AttachmentInInterview",
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
                    InterviewId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentInInterview", x => new { x.AttachmentId, x.InterviewId });
                    table.UniqueConstraint("AK_AttachmentInInterview_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachmentInInterview_Attachment_AttachmentId",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachmentInInterview_Interview_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachmentInInterview_InterviewId",
                table: "AttachmentInInterview",
                column: "InterviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentInInterview");

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentId",
                table: "Interview",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Interview_AttachmentId",
                table: "Interview",
                column: "AttachmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interview_Attachment_AttachmentId",
                table: "Interview",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
