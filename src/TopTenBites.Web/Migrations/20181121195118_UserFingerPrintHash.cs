using Microsoft.EntityFrameworkCore.Migrations;

namespace TopTenBites.Web.Migrations
{
    public partial class UserFingerPrintHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserCanvasFingerPrint",
                table: "Likes",
                newName: "UserFingerPrintHash");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserFingerPrintHash",
                table: "Likes",
                newName: "UserCanvasFingerPrint");
        }
    }
}
