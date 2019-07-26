using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class AddReporterCalendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReporterId",
                table: "Calendar",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calendar_ReporterId",
                table: "Calendar",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendar_User_ReporterId",
                table: "Calendar",
                column: "ReporterId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calendar_User_ReporterId",
                table: "Calendar");

            migrationBuilder.DropIndex(
                name: "IX_Calendar_ReporterId",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Calendar");
        }
    }
}
