﻿// <auto-generated />
using System;
using Flashcards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flashcards.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240729101927_AddStudySession")]
    partial class AddStudySession
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Flashcards.Models.Flashcard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StackId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StackId");

                    b.ToTable("Flashcards");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Answer = "Test 1",
                            Question = "Test 1",
                            StackId = 1
                        },
                        new
                        {
                            Id = 2,
                            Answer = "Test 2",
                            Question = "Test 2",
                            StackId = 1
                        });
                });

            modelBuilder.Entity("Flashcards.Models.Stack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Stacks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Questions"
                        });
                });

            modelBuilder.Entity("Flashcards.Models.StudySession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("StackId")
                        .HasColumnType("int");

                    b.Property<int>("TotalQuestions")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StackId");

                    b.ToTable("StudySessions");
                });

            modelBuilder.Entity("Flashcards.Models.Flashcard", b =>
                {
                    b.HasOne("Flashcards.Models.Stack", "Stack")
                        .WithMany("Flashcards")
                        .HasForeignKey("StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("Flashcards.Models.StudySession", b =>
                {
                    b.HasOne("Flashcards.Models.Stack", "Stack")
                        .WithMany()
                        .HasForeignKey("StackId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stack");
                });

            modelBuilder.Entity("Flashcards.Models.Stack", b =>
                {
                    b.Navigation("Flashcards");
                });
#pragma warning restore 612, 618
        }
    }
}
