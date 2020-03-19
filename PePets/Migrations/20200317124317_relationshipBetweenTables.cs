using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class relationshipBetweenTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "PetsDescription",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "PetDescriptionId",
                table: "Adverts",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "PetsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Age",
                table: "PetsDescription",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PetDescriptionId",
                table: "Adverts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "PetsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
