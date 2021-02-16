using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.DAL.Migrations
{
    public partial class ArticleVideoPropertiesPrecise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Videos",
                type: "time(0)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Videos",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time(0)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Articles",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");
        }
    }
}
