using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedWarehouse.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class remove_unit_resource_constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptResources_MeasurementUnits_MeasurementUnitId",
                table: "ReceiptResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptResources_Resources_ResourceId",
                table: "ReceiptResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptResources_MeasurementUnits_MeasurementUnitId",
                table: "ReceiptResources",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptResources_Resources_ResourceId",
                table: "ReceiptResources",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptResources_MeasurementUnits_MeasurementUnitId",
                table: "ReceiptResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptResources_Resources_ResourceId",
                table: "ReceiptResources");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptResources_MeasurementUnits_MeasurementUnitId",
                table: "ReceiptResources",
                column: "MeasurementUnitId",
                principalTable: "MeasurementUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptResources_Resources_ResourceId",
                table: "ReceiptResources",
                column: "ResourceId",
                principalTable: "Resources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
