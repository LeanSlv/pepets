using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class fixName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Pet_PetDecriptionId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDecriptionId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "PetDecriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Adverts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PetDescriptionId",
                table: "Adverts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Pet_PetDescriptionId",
                table: "Adverts",
                column: "PetDescriptionId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Pet_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDescriptionId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "PetDescriptionId",
                table: "Adverts");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Adverts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "PetDecriptionId",
                table: "Adverts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDecriptionId",
                table: "Adverts",
                column: "PetDecriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Pet_PetDecriptionId",
                table: "Adverts",
                column: "PetDecriptionId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
