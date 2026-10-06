using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dsw2025Tpi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderCustomerFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_customers_customerId1",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "customerId1",
                table: "Orders",
                newName: "customerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_customerId1",
                table: "Orders",
                newName: "IX_Orders_customerId");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_customers_customerId",
                table: "Orders",
                column: "customerId",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_customers_customerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "customerId",
                table: "Orders",
                newName: "customerId1");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_customerId",
                table: "Orders",
                newName: "IX_Orders_customerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_customers_customerId1",
                table: "Orders",
                column: "customerId1",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
