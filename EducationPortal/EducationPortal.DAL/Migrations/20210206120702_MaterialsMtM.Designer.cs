﻿// <auto-generated />
using EducationPortal.DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationPortal.DAL.Migrations
{
    [DbContext(typeof(EFDbContext))]
    [Migration("20210206120702_MaterialsMtM")]
    partial class MaterialsMtM
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("CourseMaterial", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialsId")
                        .HasColumnType("int");

                    b.HasKey("CoursesId", "MaterialsId");

                    b.HasIndex("MaterialsId");

                    b.ToTable("CourseMaterial");
                });

            modelBuilder.Entity("CourseSkill", b =>
                {
                    b.Property<int>("CoursesId")
                        .HasColumnType("int");

                    b.Property<int>("SkillsId")
                        .HasColumnType("int");

                    b.HasKey("CoursesId", "SkillsId");

                    b.HasIndex("SkillsId");

                    b.ToTable("CourseSkill");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CreatorId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserCompletedCourses", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCompletedCourses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserJoinedCourses", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserJoinedCourses");
                });

            modelBuilder.Entity("EducationPortal.DAL.Entities.EF.UserSkills", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("MaterialUser", b =>
                {
                    b.Property<int>("LearnedMaterialsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("LearnedMaterialsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("MaterialUser");
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
                    b.Navigation("CompletedCourses");

                    b.Navigation("CreatedCourses");

                    b.Navigation("JoinedCourses");

                    b.Navigation("UserSkills");
                });
#pragma warning restore 612, 618
        }
    }
}
