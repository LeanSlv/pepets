using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class UpdateAdvertModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Cost",
                table: "Adverts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Adverts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "Adverts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PetDecriptionId",
                table: "Adverts",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicationDate",
                table: "Adverts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Adverts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Adverts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UrlOfImage = table.Column<string>(nullable: true),
                    AdvertId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Adverts_AdvertId",
                        column: x => x.AdvertId,
                        principalTable: "Adverts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Sex = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Breed = table.Column<string>(nullable: true),
                    Age = table.Column<DateTime>(nullable: false),
                    Color = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_PetDecriptionId",
                table: "Adverts",
                column: "PetDecriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_AdvertId",
                table: "Image",
                column: "AdvertId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Pet_PetDecriptionId",
                table: "Adverts",
                column: "PetDecriptionId",
                principalTable: "Pet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Pet_PetDecriptionId",
                table: "Adverts");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_PetDecriptionId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "PetDecriptionId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "PublicationDate",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Adverts");

            migrationBuilder.AlterColumn<string>(
                name: "Cost",
                table: "Adverts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
