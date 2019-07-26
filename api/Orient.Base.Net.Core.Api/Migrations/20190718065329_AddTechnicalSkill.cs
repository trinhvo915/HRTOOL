using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orient.Base.Net.Core.Api.Migrations
{
    public partial class AddTechnicalSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Candidate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Candidate",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfExperienced",
                table: "Candidate",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TechnicalSkill",
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
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSkill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicalSkillInCandidate",
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
                    CandidateId = table.Column<Guid>(nullable: false),
                    TechnicalSkillId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicalSkillInCandidate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicalSkillInCandidate_Candidate_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TechnicalSkillInCandidate_TechnicalSkill_TechnicalSkillId",
                        column: x => x.TechnicalSkillId,
                        principalTable: "TechnicalSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSkillInCandidate_CandidateId",
                table: "TechnicalSkillInCandidate",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalSkillInCandidate_TechnicalSkillId",
                table: "TechnicalSkillInCandidate",
                column: "TechnicalSkillId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnicalSkillInCandidate");

            migrationBuilder.DropTable(
                name: "TechnicalSkill");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Candidate");

            migrationBuilder.DropColumn(
                name: "YearOfExperienced",
                table: "Candidate");
        }
    }
}
