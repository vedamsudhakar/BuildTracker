using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddFtpSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FtpServerId",
                table: "Builds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FtpServers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FtpServers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Builds_FtpServerId",
                table: "Builds",
                column: "FtpServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Builds_FtpServers_FtpServerId",
                table: "Builds",
                column: "FtpServerId",
                principalTable: "FtpServers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Builds_FtpServers_FtpServerId",
                table: "Builds");

            migrationBuilder.DropTable(
                name: "FtpServers");

            migrationBuilder.DropIndex(
                name: "IX_Builds_FtpServerId",
                table: "Builds");

            migrationBuilder.DropColumn(
                name: "FtpServerId",
                table: "Builds");
        }
    }
}
