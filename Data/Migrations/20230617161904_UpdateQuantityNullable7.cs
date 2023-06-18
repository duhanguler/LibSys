using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibSys.Data.Migrations
{
    public partial class UpdateQuantityNullable7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "Loans");
        }
    }
}
