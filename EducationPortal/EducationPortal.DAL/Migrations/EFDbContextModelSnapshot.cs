﻿// <auto-generated />
using System;
using EducationPortal.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationPortal.DAL.Migrations
{
    [DbContext(typeof(EFDbContext))]
    partial class EFDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CourseMaterial", b =>
                {
                    b.Property<long>("CoursesId")
                        .HasColumnType("bigint");

                    b.Property<long>("MaterialsId")
                        .HasColumnType("bigint");

                    b.HasKey("CoursesId", "MaterialsId");

                    b.HasIndex("MaterialsId");

                    b.ToTable("CourseMaterial");
                });

            modelBuilder.Entity("CourseSkill", b =>
                {
                    b.Property<long>("CoursesId")
                        .HasColumnType("bigint");

                    b.Property<long>("SkillsId")
                        .HasColumnType("bigint");

                    b.HasKey("CoursesId", "SkillsId");

                    b.HasIndex("SkillsId");

                    b.ToTable("CourseSkill");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Login")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Course", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Material", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Skill", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserCompletedCourses", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCompletedCourses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserJoinedCourses", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("CourseId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserJoinedCourses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserSkills", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("SkillId")
                        .HasColumnType("bigint");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("MaterialUser", b =>
                {
                    b.Property<long>("LearnedMaterialsId")
                        .HasColumnType("bigint");

                    b.Property<long>("UsersId")
                        .HasColumnType("bigint");

                    b.HasKey("LearnedMaterialsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MaterialUser");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Article", b =>
                {
                    b.HasBaseType("EducationPortal.DAL.Entities.EF.Material");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("date");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Book", b =>
                {
                    b.HasBaseType("EducationPortal.DAL.Entities.EF.Material");

                    b.Property<string>("AuthorNames")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Format")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("PageCount")
                        .HasColumnType("int");

                    b.Property<short>("PublishingYear")
                        .HasColumnType("smallint");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Video", b =>
                {
                    b.HasBaseType("EducationPortal.DAL.Entities.EF.Material");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time(0)");

                    b.Property<string>("Quality")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("CourseMaterial", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.Material", null)
                        .WithMany()
                        .HasForeignKey("MaterialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CourseSkill", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.Skill", null)
                        .WithMany()
                        .HasForeignKey("SkillsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Account", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("EducationPortal.DAL.Entities.EF.Account", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Course", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.User", "Creator")
                        .WithMany("CreatedCourses")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserCompletedCourses", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Course", "Course")
                        .WithMany("CompletedUsers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.User", "User")
                        .WithMany("CompletedCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserJoinedCourses", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Course", "Course")
                        .WithMany("JoinedUsers")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.User", "User")
                        .WithMany("JoinedCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserSkills", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Skill", "Skill")
                        .WithMany("UserSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.User", "User")
                        .WithMany("UserSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MaterialUser", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Material", null)
                        .WithMany()
                        .HasForeignKey("LearnedMaterialsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.DAL.Entities.EF.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Article", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.DAL.Entities.EF.Article", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Book", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.DAL.Entities.EF.Book", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Video", b =>
                {
                    b.HasOne("EducationPortal.DAL.Entities.EF.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.DAL.Entities.EF.Video", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Course", b =>
                {
                    b.Navigation("CompletedUsers");

                    b.Navigation("JoinedUsers");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Skill", b =>
                {
                    b.Navigation("UserSkills");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.User", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("CompletedCourses");

                    b.Navigation("CreatedCourses");

                    b.Navigation("JoinedCourses");

                    b.Navigation("UserSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
