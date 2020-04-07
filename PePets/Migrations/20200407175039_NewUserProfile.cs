using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class NewUserProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Adverts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_UserId1",
                table: "Adverts",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_AspNetUsers_UserId1",
                table: "Adverts",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_AspNetUsers_UserId1",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_UserId1",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecondName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Adverts");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "Adverts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId1",
                table: "Adverts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
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
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

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
    }
}
