using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTracker.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBuildType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "BuildType",
                table: "Builds");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Builds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationId",
                table: "Builds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BuildType",
                table: "Builds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
