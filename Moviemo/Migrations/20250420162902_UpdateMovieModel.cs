using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moviemo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovieModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "movieId",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_movieId",
                table: "Comments",
                column: "movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Movies_movieId",
                table: "Comments",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "movieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Movies_movieId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_movieId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "movieId",
                table: "Comments");
        }
    }
}
