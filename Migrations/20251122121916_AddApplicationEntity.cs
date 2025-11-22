using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Builds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Builds_ApplicationId",
                table: "Builds",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");

            // Seed Applications
            migrationBuilder.Sql("INSERT INTO Applications (Name) VALUES ('2D')");
            migrationBuilder.Sql("INSERT INTO Applications (Name) VALUES ('3D')");
            migrationBuilder.Sql("INSERT INTO Applications (Name) VALUES ('Weld Inspect')");

            // Migrate Data
            migrationBuilder.Sql("UPDATE Builds SET ApplicationId = (SELECT Id FROM Applications WHERE Name = '2D') WHERE BuildType = 0");
            migrationBuilder.Sql("UPDATE Builds SET ApplicationId = (SELECT Id FROM Applications WHERE Name = '3D') WHERE BuildType = 1");
            migrationBuilder.Sql("UPDATE Builds SET ApplicationId = (SELECT Id FROM Applications WHERE Name = 'Weld Inspect') WHERE BuildType = 2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Builds_Applications_ApplicationId",
                table: "Builds");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Builds_ApplicationId",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Builds");
        }
    }
}
