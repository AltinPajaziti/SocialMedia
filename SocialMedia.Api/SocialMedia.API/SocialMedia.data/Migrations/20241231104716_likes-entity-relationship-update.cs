using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.data.Migrations
{
    /// <inheritdoc />
    public partial class likesentityrelationshipupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Postid",
                table: "Likes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Id_Postid",
                table: "Likes",
                columns: new[] { "Id", "Postid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Likes_Postid",
                table: "Likes",
                column: "Postid");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Posts_Postid",
                table: "Likes",
                column: "Postid",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Posts_Postid",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_Id_Postid",
                table: "Likes");

            migrationBuilder.DropIndex(
                name: "IX_Likes_Postid",
                table: "Likes");

            migrationBuilder.DropColumn(
                name: "Postid",
                table: "Likes");
        }
    }
}
