using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserLoginHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugHistories_AspNetUsers_ChangedByUserId",
                table: "BugHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_BugHistories_Bugs_BugId",
                table: "BugHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BugHistories",
                table: "BugHistories");

            migrationBuilder.RenameTable(
                name: "BugHistories",
                newName: "BugHistory");

            migrationBuilder.RenameIndex(
                name: "IX_BugHistories_ChangedByUserId",
                table: "BugHistory",
                newName: "IX_BugHistory_ChangedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BugHistories_BugId",
                table: "BugHistory",
                newName: "IX_BugHistory_BugId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BugHistory",
                table: "BugHistory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserLoginHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_UserId",
                table: "UserLoginHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BugHistory_AspNetUsers_ChangedByUserId",
                table: "BugHistory",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BugHistory_Bugs_BugId",
                table: "BugHistory",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BugHistory_AspNetUsers_ChangedByUserId",
                table: "BugHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_BugHistory_Bugs_BugId",
                table: "BugHistory");

            migrationBuilder.DropTable(
                name: "UserLoginHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BugHistory",
                table: "BugHistory");

            migrationBuilder.RenameTable(
                name: "BugHistory",
                newName: "BugHistories");

            migrationBuilder.RenameIndex(
                name: "IX_BugHistory_ChangedByUserId",
                table: "BugHistories",
                newName: "IX_BugHistories_ChangedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BugHistory_BugId",
                table: "BugHistories",
                newName: "IX_BugHistories_BugId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BugHistories",
                table: "BugHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BugHistories_AspNetUsers_ChangedByUserId",
                table: "BugHistories",
                column: "ChangedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BugHistories_Bugs_BugId",
                table: "BugHistories",
                column: "BugId",
                principalTable: "Bugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
