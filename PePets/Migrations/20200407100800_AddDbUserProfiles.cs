using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class AddDbUserProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "Adverts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId1",
                table: "Adverts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SecondName = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_UserProfileId",
                table: "Adverts",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_UserProfileId1",
                table: "Adverts",
                column: "UserProfileId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_UserProfiles_UserProfileId",
                table: "Adverts",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_UserProfiles_UserProfileId1",
                table: "Adverts",
                column: "UserProfileId1",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_UserProfiles_UserProfileId",
                table: "Adverts");

            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_UserProfiles_UserProfileId1",
                table: "Adverts");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_UserProfileId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_UserProfileId1",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "UserProfileId1",
                table: "Adverts");
        }
    }
}
