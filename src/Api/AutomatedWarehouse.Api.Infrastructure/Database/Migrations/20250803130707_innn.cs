using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedWarehouse.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class innn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReceiptNumber",
                table: "ReceiptDocuments",
                type: "text",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ReceiptNumber",
                table: "ReceiptDocuments",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
