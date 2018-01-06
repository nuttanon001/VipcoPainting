using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddStockModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionMaster_TaskPaintDetail_TaskPaintDetailId",
                table: "RequisitionMaster");

            migrationBuilder.RenameColumn(
                name: "TaskPaintDetailId",
                table: "RequisitionMaster",
                newName: "ColorMovementStockId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitionMaster_TaskPaintDetailId",
                table: "RequisitionMaster",
                newName: "IX_RequisitionMaster_ColorMovementStockId");

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "RequisitionMaster",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MovementStockStatus",
                columns: table => new
                {
                    MovementStockStatusId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    StatusMovement = table.Column<int>(nullable: false),
                    StatusName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovementStockStatus", x => x.MovementStockStatusId);
                });

            migrationBuilder.CreateTable(
                name: "ColorMovementStock",
                columns: table => new
                {
                    ColortMovementStockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    MovementStockDate = table.Column<DateTime>(nullable: false),
                    MovementStockStatusId = table.Column<int>(nullable: true),
                    Quantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorMovementStock", x => x.ColortMovementStockId);
                    table.ForeignKey(
                        name: "FK_ColorMovementStock_MovementStockStatus_MovementStockStatusId",
                        column: x => x.MovementStockStatusId,
                        principalTable: "MovementStockStatus",
                        principalColumn: "MovementStockStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinishedGoodsMaster",
                columns: table => new
                {
                    FinishedGoodsMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorItemId = table.Column<int>(nullable: true),
                    ColorMovementStockId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    FinishedGoodsDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: true),
                    ReceiveBy = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinishedGoodsMaster", x => x.FinishedGoodsMasterId);
                    table.ForeignKey(
                        name: "FK_FinishedGoodsMaster_ColorItem_ColorItemId",
                        column: x => x.ColorItemId,
                        principalTable: "ColorItem",
                        principalColumn: "ColorItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinishedGoodsMaster_ColorMovementStock_ColorMovementStockId",
                        column: x => x.ColorMovementStockId,
                        principalTable: "ColorMovementStock",
                        principalColumn: "ColortMovementStockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorMovementStock_MovementStockStatusId",
                table: "ColorMovementStock",
                column: "MovementStockStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoodsMaster_ColorItemId",
                table: "FinishedGoodsMaster",
                column: "ColorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FinishedGoodsMaster_ColorMovementStockId",
                table: "FinishedGoodsMaster",
                column: "ColorMovementStockId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionMaster_ColorMovementStock_ColorMovementStockId",
                table: "RequisitionMaster",
                column: "ColorMovementStockId",
                principalTable: "ColorMovementStock",
                principalColumn: "ColortMovementStockId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionMaster_ColorMovementStock_ColorMovementStockId",
                table: "RequisitionMaster");

            migrationBuilder.DropTable(
                name: "FinishedGoodsMaster");

            migrationBuilder.DropTable(
                name: "ColorMovementStock");

            migrationBuilder.DropTable(
                name: "MovementStockStatus");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "RequisitionMaster");

            migrationBuilder.RenameColumn(
                name: "ColorMovementStockId",
                table: "RequisitionMaster",
                newName: "TaskPaintDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_RequisitionMaster_ColorMovementStockId",
                table: "RequisitionMaster",
                newName: "IX_RequisitionMaster_TaskPaintDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionMaster_TaskPaintDetail_TaskPaintDetailId",
                table: "RequisitionMaster",
                column: "TaskPaintDetailId",
                principalTable: "TaskPaintDetail",
                principalColumn: "TaskPaintDetailId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
