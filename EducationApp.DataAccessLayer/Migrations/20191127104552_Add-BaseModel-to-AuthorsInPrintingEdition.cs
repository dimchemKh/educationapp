using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class AddBaseModeltoAuthorsInPrintingEdition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "AuthorInPrintingEditions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRemoved",
                table: "AuthorInPrintingEditions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "AuthorInPrintingEditions");

            migrationBuilder.DropColumn(
                name: "IsRemoved",
                table: "AuthorInPrintingEditions");
        }
    }
}
