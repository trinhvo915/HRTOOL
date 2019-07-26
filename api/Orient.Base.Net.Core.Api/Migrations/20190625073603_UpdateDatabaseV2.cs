using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class UpdateDatabaseV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeInInterview");

            migrationBuilder.DropTable(
                name: "Employee");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Birthday = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(maxLength: 512, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    LevelEmp = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 512, nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeInInterview",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(nullable: false),
                    InterviewId = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<Guid>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    RecordActive = table.Column<bool>(nullable: false),
                    RecordDeleted = table.Column<bool>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<Guid>(nullable: true),
                    UpdatedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeInInterview", x => new { x.EmployeeId, x.InterviewId });
                    table.UniqueConstraint("AK_EmployeeInInterview_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeInInterview_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeInInterview_Interview_InterviewId",
                        column: x => x.InterviewId,
                        principalTable: "Interview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeInInterview_InterviewId",
                table: "EmployeeInInterview",
                column: "InterviewId");
        }
    }
}
