using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutomatedWarehouse.Api.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeasurementUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasurementUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptNumber = table.Column<long>(type: "bigint", nullable: false),
                    ReceiptDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeasurementUnitId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    ReceiptDocumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiptDocumentId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptResources_MeasurementUnits_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "MeasurementUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReceiptResources_ReceiptDocuments_ReceiptDocumentId",
                        column: x => x.ReceiptDocumentId,
                        principalTable: "ReceiptDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptResources_ReceiptDocuments_ReceiptDocumentId1",
                        column: x => x.ReceiptDocumentId1,
                        principalTable: "ReceiptDocuments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptResources_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeasurementUnits_Name",
                table: "MeasurementUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptDocuments_ReceiptNumber",
                table: "ReceiptDocuments",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptResources_MeasurementUnitId",
                table: "ReceiptResources",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptResources_ReceiptDocumentId",
                table: "ReceiptResources",
                column: "ReceiptDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptResources_ReceiptDocumentId1",
                table: "ReceiptResources",
                column: "ReceiptDocumentId1");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptResources_ResourceId",
                table: "ReceiptResources",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Name",
                table: "Resources",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiptResources");

            migrationBuilder.DropTable(
                name: "MeasurementUnits");

            migrationBuilder.DropTable(
                name: "ReceiptDocuments");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
