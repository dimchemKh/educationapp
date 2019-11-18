using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationApp.DataAccessLayer.Migrations
{
    public partial class NewDbName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TransactionId",
                table: "Payments",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
