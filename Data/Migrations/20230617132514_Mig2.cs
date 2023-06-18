using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSys.Data.Migrations
{
    public partial class Mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "dbo.Inventory",
                nullable: true,
                oldNullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
