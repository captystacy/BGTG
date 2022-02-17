using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BGTG.POS.Data.Migrations
{
    public partial class AddIndexToConstructionObjectCipher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ConstructionObjects_Cipher",
                table: "ConstructionObjects",
                column: "Cipher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ConstructionObjects_Cipher",
                table: "ConstructionObjects");
        }
    }
}
