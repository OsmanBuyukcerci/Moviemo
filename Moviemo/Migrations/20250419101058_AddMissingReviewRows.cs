using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moviemo.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingReviewRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_userId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_userId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Reviews");

            migrationBuilder.AddColumn<long>(
                name: "authoruserId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "reviewedMoviemovieId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_authoruserId",
                table: "Reviews",
                column: "authoruserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_reviewedMoviemovieId",
                table: "Reviews",
                column: "reviewedMoviemovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_reviewedMoviemovieId",
                table: "Reviews",
                column: "reviewedMoviemovieId",
                principalTable: "Movies",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_authoruserId",
                table: "Reviews",
                column: "authoruserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_reviewedMoviemovieId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_authoruserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_authoruserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_reviewedMoviemovieId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "authoruserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "reviewedMoviemovieId",
                table: "Reviews");

            migrationBuilder.AddColumn<long>(
                name: "userId",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_userId",
                table: "Reviews",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_userId",
                table: "Reviews",
                column: "userId",
                principalTable: "Users",
                principalColumn: "userId");
        }
    }
}
