﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moviemo.Data;

#nullable disable

namespace Moviemo.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Moviemo.Models.Comment", b =>
                {
                    b.Property<long>("commentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("commentId"));

                    b.Property<long>("authoruserId")
                        .HasColumnType("bigint");

                    b.Property<string>("body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("dislikeCounter")
                        .HasColumnType("bigint");

                    b.Property<long>("likeCounter")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("commentId");

                    b.HasIndex("authoruserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Moviemo.Models.Movie", b =>
                {
                    b.Property<long>("movieId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("movieId"));

                    b.Property<string>("overview")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("posterPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("tmdbScore")
                        .HasColumnType("float");

                    b.Property<string>("trailerUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("movieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Moviemo.Models.Report", b =>
                {
                    b.Property<long>("reportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("reportId"));

                    b.Property<long>("authoruserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("details")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("reportId");

                    b.HasIndex("authoruserId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("Moviemo.Models.Review", b =>
                {
                    b.Property<long>("reviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("reviewId"));

                    b.Property<string>("body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdAt")
                        .HasColumnType("datetime2");

                    b.Property<long>("dislikeCounter")
                        .HasColumnType("bigint");

                    b.Property<long>("likeCounter")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("updatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("userId")
                        .HasColumnType("bigint");

                    b.Property<double>("userScore")
                        .HasColumnType("float");

                    b.HasKey("reviewId");

                    b.HasIndex("userId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Moviemo.Models.User", b =>
                {
                    b.Property<long>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("userId"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userRole")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("userId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Moviemo.Models.Comment", b =>
                {
                    b.HasOne("Moviemo.Models.User", "author")
                        .WithMany("comments")
                        .HasForeignKey("authoruserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("author");
                });

            modelBuilder.Entity("Moviemo.Models.Report", b =>
                {
                    b.HasOne("Moviemo.Models.User", "author")
                        .WithMany()
                        .HasForeignKey("authoruserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("author");
                });

            modelBuilder.Entity("Moviemo.Models.Review", b =>
                {
                    b.HasOne("Moviemo.Models.User", null)
                        .WithMany("reviews")
                        .HasForeignKey("userId");
                });

            modelBuilder.Entity("Moviemo.Models.User", b =>
                {
                    b.Navigation("comments");

                    b.Navigation("reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
