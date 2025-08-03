using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedWarehouse.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class hhh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptResources_ReceiptDocuments_ReceiptDocumentId1",
                table: "ReceiptResources");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptResources_ReceiptDocumentId1",
                table: "ReceiptResources");

            migrationBuilder.DropColumn(
                name: "ReceiptDocumentId1",
                table: "ReceiptResources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReceiptDocumentId1",
                table: "ReceiptResources",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptResources_ReceiptDocumentId1",
                table: "ReceiptResources",
                column: "ReceiptDocumentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptResources_ReceiptDocuments_ReceiptDocumentId1",
                table: "ReceiptResources",
                column: "ReceiptDocumentId1",
                principalTable: "ReceiptDocuments",
                principalColumn: "Id");
        }
    }
}
