using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddInitialRequireModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InitialRequireId",
                table: "PaintWorkItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InitialRequireId",
                table: "BlastWorkItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InitialRequirePaintingList",
                columns: table => new
                {
                    InitialRequireId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    DrawingNo = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PlanEnd = table.Column<DateTime>(nullable: true),
                    PlanStart = table.Column<DateTime>(nullable: true),
                    RequirePaintingMasterId = table.Column<int>(nullable: true),
                    UnitNo = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitialRequirePaintingList", x => x.InitialRequireId);
                    table.ForeignKey(
                        name: "FK_InitialRequirePaintingList_RequirePaintingMaster_RequirePaintingMasterId",
                        column: x => x.RequirePaintingMasterId,
                        principalTable: "RequirePaintingMaster",
                        principalColumn: "RequirePaintingMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_InitialRequireId",
                table: "PaintWorkItem",
                column: "InitialRequireId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_InitialRequireId",
                table: "BlastWorkItem",
                column: "InitialRequireId");

            migrationBuilder.CreateIndex(
                name: "IX_InitialRequirePaintingList_RequirePaintingMasterId",
                table: "InitialRequirePaintingList",
                column: "RequirePaintingMasterId",
                unique: true,
                filter: "[RequirePaintingMasterId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BlastWorkItem_InitialRequirePaintingList_InitialRequireId",
                table: "BlastWorkItem",
                column: "InitialRequireId",
                principalTable: "InitialRequirePaintingList",
                principalColumn: "InitialRequireId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PaintWorkItem_InitialRequirePaintingList_InitialRequireId",
                table: "PaintWorkItem",
                column: "InitialRequireId",
                principalTable: "InitialRequirePaintingList",
                principalColumn: "InitialRequireId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlastWorkItem_InitialRequirePaintingList_InitialRequireId",
                table: "BlastWorkItem");

            migrationBuilder.DropForeignKey(
                name: "FK_PaintWorkItem_InitialRequirePaintingList_InitialRequireId",
                table: "PaintWorkItem");

            migrationBuilder.DropTable(
                name: "InitialRequirePaintingList");

            migrationBuilder.DropIndex(
                name: "IX_PaintWorkItem_InitialRequireId",
                table: "PaintWorkItem");

            migrationBuilder.DropIndex(
                name: "IX_BlastWorkItem_InitialRequireId",
                table: "BlastWorkItem");

            migrationBuilder.DropColumn(
                name: "InitialRequireId",
                table: "PaintWorkItem");

            migrationBuilder.DropColumn(
                name: "InitialRequireId",
                table: "BlastWorkItem");
        }
    }
}
