using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantSignalServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStrategyUserIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signals_Strategies_StrategyName",
                table: "Signals");

            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_Users_UserId1",
                table: "Strategies");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Strategies_Name",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_UserId1",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_Signals_StrategyName",
                table: "Signals");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Strategies");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Strategies",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Signals",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_UserId",
                table: "Strategies",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_Users_UserId",
                table: "Strategies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Strategies_Users_UserId",
                table: "Strategies");

            migrationBuilder.DropIndex(
                name: "IX_Strategies_UserId",
                table: "Strategies");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Strategies",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Strategies",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Signals",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Strategies_Name",
                table: "Strategies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Strategies_UserId1",
                table: "Strategies",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Signals_StrategyName",
                table: "Signals",
                column: "StrategyName");

            migrationBuilder.AddForeignKey(
                name: "FK_Signals_Strategies_StrategyName",
                table: "Signals",
                column: "StrategyName",
                principalTable: "Strategies",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Strategies_Users_UserId1",
                table: "Strategies",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
