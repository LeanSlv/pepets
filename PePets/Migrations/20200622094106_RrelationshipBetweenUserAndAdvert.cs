using Microsoft.EntityFrameworkCore.Migrations;

namespace PePets.Migrations
{
    public partial class RrelationshipBetweenUserAndAdvert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_AspNetUsers_UserId1",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_UserId1",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Adverts");

            migrationBuilder.AddColumn<string>(
                name: "UserFavoritesId",
                table: "Adverts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_UserFavoritesId",
                table: "Adverts",
                column: "UserFavoritesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_AspNetUsers_UserFavoritesId",
                table: "Adverts",
                column: "UserFavoritesId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_AspNetUsers_UserFavoritesId",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_UserFavoritesId",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "UserFavoritesId",
                table: "Adverts");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Adverts",
                type: "nvarchar(450)",
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
    }
}
