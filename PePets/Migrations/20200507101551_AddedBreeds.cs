using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class AddedBreeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Dogs = table.Column<string>(nullable: true),
                    Cats = table.Column<string>(nullable: true),
                    Birds = table.Column<string>(nullable: true),
                    Rodents = table.Column<string>(nullable: true),
                    Fishes = table.Column<string>(nullable: true),
                    FarmAnimals = table.Column<string>(nullable: true),
                    Other = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Breeds");
        }
    }
}
