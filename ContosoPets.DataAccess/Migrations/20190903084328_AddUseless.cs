using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoPets.DataAccess.Migrations
{
    public partial class AddUseless : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Useless",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Useless",
                table: "Customers");
        }
    }
}
