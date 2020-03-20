using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class AnotherRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AddColumn<Guid>(
                name: "AdvertId",
                table: "PetsDescription",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PetsDescription_AdvertId",
                table: "PetsDescription",
                column: "AdvertId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PetsDescription_Adverts_AdvertId",
                table: "PetsDescription",
                column: "AdvertId",
                principalTable: "Adverts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PetsDescription_Adverts_AdvertId",
                table: "PetsDescription");

            migrationBuilder.DropIndex(
                name: "IX_PetsDescription_AdvertId",
                table: "PetsDescription");

            migrationBuilder.DropColumn(
                name: "AdvertId",
                table: "PetsDescription");

            migrationBuilder.AddColumn<Guid>(
                name: "PetDescriptionId",
                table: "Adverts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_PetsDescription_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "PetsDescription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
