using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateModelMovementStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequisitionMaster_ColorMovementStockId",
                table: "RequisitionMaster");

            migrationBuilder.DropIndex(
                name: "IX_FinishedGoodsMaster_ColorMovementStockId",
                table: "FinishedGoodsMaster");

            migrationBuilder.AddColumn<int>(
                name: "TypeStatusMovement",
                table: "MovementStockStatus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorItemId",
                table: "ColorMovementStock",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_ColorMovementStockId",
                table: "RequisitionMaster",
                column: "ColorMovementStockId",
                unique: true,
                filter: "[ColorMovementStockId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoodsMaster_ColorMovementStockId",
                table: "FinishedGoodsMaster",
                column: "ColorMovementStockId",
                unique: true,
                filter: "[ColorMovementStockId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ColorMovementStock_ColorItemId",
                table: "ColorMovementStock",
                column: "ColorItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ColorMovementStock_ColorItem_ColorItemId",
                table: "ColorMovementStock",
                column: "ColorItemId",
                principalTable: "ColorItem",
                principalColumn: "ColorItemId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ColorMovementStock_ColorItem_ColorItemId",
                table: "ColorMovementStock");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionMaster_ColorMovementStockId",
                table: "RequisitionMaster");

            migrationBuilder.DropIndex(
                name: "IX_FinishedGoodsMaster_ColorMovementStockId",
                table: "FinishedGoodsMaster");

            migrationBuilder.DropIndex(
                name: "IX_ColorMovementStock_ColorItemId",
                table: "ColorMovementStock");

            migrationBuilder.DropColumn(
                name: "TypeStatusMovement",
                table: "MovementStockStatus");

            migrationBuilder.DropColumn(
                name: "ColorItemId",
                table: "ColorMovementStock");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_ColorMovementStockId",
                table: "RequisitionMaster",
                column: "ColorMovementStockId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoodsMaster_ColorMovementStockId",
                table: "FinishedGoodsMaster",
                column: "ColorMovementStockId");
        }
    }
}
