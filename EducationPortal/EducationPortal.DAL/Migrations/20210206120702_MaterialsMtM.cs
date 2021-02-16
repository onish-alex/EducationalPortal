using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationPortal.DAL.Migrations
{
    public partial class MaterialsMtM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Courses_CourseId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Users_UserId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_CourseId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_UserId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Materials");

            migrationBuilder.CreateTable(
                name: "CourseMaterial",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    MaterialsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseMaterial", x => new { x.CoursesId, x.MaterialsId });
                    table.ForeignKey(
                        name: "FK_CourseMaterial_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseMaterial_Materials_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialUser",
                columns: table => new
                {
                    LearnedMaterialsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialUser", x => new { x.LearnedMaterialsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_MaterialUser_Materials_LearnedMaterialsId",
                        column: x => x.LearnedMaterialsId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseMaterial_MaterialsId",
                table: "CourseMaterial",
                column: "MaterialsId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialUser_UsersId",
                table: "MaterialUser",
                column: "UsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseMaterial");

            migrationBuilder.DropTable(
                name: "MaterialUser");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CourseId",
                table: "Materials",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_UserId",
                table: "Materials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Courses_CourseId",
                table: "Materials",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Users_UserId",
                table: "Materials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
