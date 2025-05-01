using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moviemo.Migrations
{
    /// <inheritdoc />
    public partial class VoteAddedRelationshipsSettedPlusImprovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Movies_movieId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_authoruserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_authoruserId",
                table: "Reports");

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
                name: "IX_Comments_authoruserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "authoruserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "dislikeCounter",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "authoruserId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "dislikeCounter",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "userRole",
                table: "Users",
                newName: "UserRole");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "Users",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "userScore",
                table: "Reviews",
                newName: "UserScore");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Reviews",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Reviews",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Reviews",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "reviewId",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "reviewedMoviemovieId",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "likeCounter",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_reviewedMoviemovieId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Reports",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "details",
                table: "Reports",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Reports",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "reportId",
                table: "Reports",
                newName: "ReportId");

            migrationBuilder.RenameColumn(
                name: "authoruserId",
                table: "Reports",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_authoruserId",
                table: "Reports",
                newName: "IX_Reports_UserId");

            migrationBuilder.RenameColumn(
                name: "trailerUrl",
                table: "Movies",
                newName: "TrailerUrl");

            migrationBuilder.RenameColumn(
                name: "tmdbScore",
                table: "Movies",
                newName: "TmdbScore");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Movies",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "posterPath",
                table: "Movies",
                newName: "PosterPath");

            migrationBuilder.RenameColumn(
                name: "overview",
                table: "Movies",
                newName: "Overview");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "Movies",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "updatedAt",
                table: "Comments",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "Comments",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Comments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Comments",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "commentId",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.RenameColumn(
                name: "likeCounter",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_movieId",
                table: "Comments",
                newName: "IX_Comments_MovieId");

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Reviews",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Reports",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "MovieId",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MovieId1",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Comments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    VoteType = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentId = table.Column<long>(type: "bigint", nullable: false),
                    ReviewId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "Reviews",
                        principalColumn: "ReviewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId1",
                table: "Reviews",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId1",
                table: "Reports",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MovieId1",
                table: "Comments",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId1",
                table: "Comments",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_CommentId",
                table: "Votes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_ReviewId",
                table: "Votes",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Movies_MovieId",
                table: "Comments",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Movies_MovieId1",
                table: "Comments",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_UserId1",
                table: "Reports",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "MovieId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId1",
                table: "Reviews",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId1",
                table: "Reviews",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Movies_MovieId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Movies_MovieId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Users_UserId1",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId1",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId1",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reports_UserId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Comments_MovieId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "UserRole",
                table: "Users",
                newName: "userRole");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "Users",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "UserScore",
                table: "Reviews",
                newName: "userScore");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Reviews",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reviews",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Reviews",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "reviewId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "reviewedMoviemovieId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "likeCounter");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_reviewedMoviemovieId");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Reports",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "Reports",
                newName: "details");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reports",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "Reports",
                newName: "reportId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reports",
                newName: "authoruserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                newName: "IX_Reports_authoruserId");

            migrationBuilder.RenameColumn(
                name: "TrailerUrl",
                table: "Movies",
                newName: "trailerUrl");

            migrationBuilder.RenameColumn(
                name: "TmdbScore",
                table: "Movies",
                newName: "tmdbScore");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Movies",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "PosterPath",
                table: "Movies",
                newName: "posterPath");

            migrationBuilder.RenameColumn(
                name: "Overview",
                table: "Movies",
                newName: "overview");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Movies",
                newName: "movieId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Comments",
                newName: "updatedAt");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Comments",
                newName: "movieId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Comments",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "Comments",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "commentId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "likeCounter");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_MovieId",
                table: "Comments",
                newName: "IX_Comments_movieId");

            migrationBuilder.AddColumn<long>(
                name: "authoruserId",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "dislikeCounter",
                table: "Reviews",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "movieId",
                table: "Comments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "authoruserId",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "dislikeCounter",
                table: "Comments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_authoruserId",
                table: "Reviews",
                column: "authoruserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_authoruserId",
                table: "Comments",
                column: "authoruserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Movies_movieId",
                table: "Comments",
                column: "movieId",
                principalTable: "Movies",
                principalColumn: "movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_authoruserId",
                table: "Comments",
                column: "authoruserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Users_authoruserId",
                table: "Reports",
                column: "authoruserId",
                principalTable: "Users",
                principalColumn: "userId",
                onDelete: ReferentialAction.Cascade);

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
    }
}
