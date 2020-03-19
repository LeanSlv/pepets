using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class fixbug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId");
        }
    }
}
