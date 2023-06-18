using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSys.Data.Migrations
{
    public partial class UpdateQuantityNullable9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Resource",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Resource");
        }
    }
}
