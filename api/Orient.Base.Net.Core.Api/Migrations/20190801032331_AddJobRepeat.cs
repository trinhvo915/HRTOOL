using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class AddJobRepeat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DateRepeat",
                table: "Job",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "IdLink",
                table: "Job",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisable",
                table: "Job",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsProcess",
                table: "Job",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRepeat",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IdLink",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsDisable",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "IsProcess",
                table: "Job");
        }
    }
}
