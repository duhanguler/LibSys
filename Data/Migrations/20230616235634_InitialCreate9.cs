using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSys.Data.Migrations
{
    public partial class InitialCreate9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailableForBorrowing",
                table: "Resource",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LoanPeriod",
                table: "Resource",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailableForBorrowing",
                table: "Resource");

            migrationBuilder.DropColumn(
                name: "LoanPeriod",
                table: "Resource");
        }
    }
}
