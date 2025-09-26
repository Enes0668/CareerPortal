using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KariyerPortali.Migrations
{
    /// <inheritdoc />
    public partial class Newsh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobPostingId",
                table: "Applications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobPostingId",
                table: "Applications",
                column: "JobPostingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobPostings_JobPostingId",
                table: "Applications",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobPostings_JobPostingId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_JobPostingId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "JobPostingId",
                table: "Applications");
        }
    }
}
