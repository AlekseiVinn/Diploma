using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScopeLap.Migrations
{
    /// <inheritdoc />
    public partial class postupdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_AccountID",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "AccountID",
                table: "Posts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_AccountID",
                table: "Posts",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Accounts_AccountID",
                table: "Posts");

            migrationBuilder.AlterColumn<int>(
                name: "AccountID",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Accounts_AccountID",
                table: "Posts",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
