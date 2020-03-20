using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class newTypeGenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<int>(
                name: "Sex",
                table: "PetsDescription",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "PetsDescription",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                unique: true);
        }
    }
}
