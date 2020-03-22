using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class AddPetsDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Pet_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pet",
                table: "Pet");

            migrationBuilder.RenameTable(
                name: "Pet",
                newName: "PetsDescription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PetsDescription",
                table: "PetsDescription",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "PetsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PetsDescription",
                table: "PetsDescription");

            migrationBuilder.RenameTable(
                name: "PetsDescription",
                newName: "Pet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pet",
                table: "Pet",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Pet_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
