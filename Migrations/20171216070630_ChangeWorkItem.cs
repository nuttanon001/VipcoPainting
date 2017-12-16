using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class ChangeWorkItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirePaintingSub");

            migrationBuilder.CreateTable(
                name: "BlastWorkItem",
                columns: table => new
                {
                    BlastWorkItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExtArea = table.Column<double>(nullable: true),
                    ExtCalcStdUsage = table.Column<double>(nullable: true),
                    IntArea = table.Column<double>(nullable: true),
                    IntCalcStdUsage = table.Column<double>(nullable: true),
                    RequirePaintingListId = table.Column<int>(nullable: true),
                    StandradTimeExtId = table.Column<int>(nullable: true),
                    StandradTimeIntId = table.Column<int>(nullable: true),
                    SurfaceTypeExtId = table.Column<int>(nullable: true),
                    SurfaceTypeIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastWorkItem", x => x.BlastWorkItemId);
                    table.ForeignKey(
                        name: "FK_BlastWorkItem_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastWorkItem_StandradTime_StandradTimeExtId",
                        column: x => x.StandradTimeExtId,
                        principalTable: "StandradTime",
                        principalColumn: "StandradTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastWorkItem_StandradTime_StandradTimeIntId",
                        column: x => x.StandradTimeIntId,
                        principalTable: "StandradTime",
                        principalColumn: "StandradTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastWorkItem_SurfaceType_SurfaceTypeExtId",
                        column: x => x.SurfaceTypeExtId,
                        principalTable: "SurfaceType",
                        principalColumn: "SurfaceTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlastWorkItem_SurfaceType_SurfaceTypeIntId",
                        column: x => x.SurfaceTypeIntId,
                        principalTable: "SurfaceType",
                        principalColumn: "SurfaceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaintWorkItem",
                columns: table => new
                {
                    PaintWorkItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ExtArea = table.Column<double>(nullable: true),
                    ExtCalcColorUsage = table.Column<double>(nullable: true),
                    ExtCalcStdUsage = table.Column<double>(nullable: true),
                    ExtColorItemId = table.Column<int>(nullable: true),
                    ExtDFTMax = table.Column<double>(nullable: true),
                    ExtDFTMin = table.Column<double>(nullable: true),
                    IntArea = table.Column<double>(nullable: true),
                    IntCalcColorUsage = table.Column<double>(nullable: true),
                    IntCalcStdUsage = table.Column<double>(nullable: true),
                    IntColorItemId = table.Column<int>(nullable: true),
                    IntDFTMax = table.Column<double>(nullable: true),
                    IntDFTMin = table.Column<double>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintLevel = table.Column<int>(nullable: true),
                    RequirePaintingListId = table.Column<int>(nullable: true),
                    StandradTimeExtId = table.Column<int>(nullable: true),
                    StandradTimeIntId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintWorkItem", x => x.PaintWorkItemId);
                    table.ForeignKey(
                        name: "FK_PaintWorkItem_ColorItem_ExtColorItemId",
                        column: x => x.ExtColorItemId,
                        principalTable: "ColorItem",
                        principalColumn: "ColorItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintWorkItem_ColorItem_IntColorItemId",
                        column: x => x.IntColorItemId,
                        principalTable: "ColorItem",
                        principalColumn: "ColorItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintWorkItem_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintWorkItem_StandradTime_StandradTimeExtId",
                        column: x => x.StandradTimeExtId,
                        principalTable: "StandradTime",
                        principalColumn: "StandradTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintWorkItem_StandradTime_StandradTimeIntId",
                        column: x => x.StandradTimeIntId,
                        principalTable: "StandradTime",
                        principalColumn: "StandradTimeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_RequirePaintingListId",
                table: "BlastWorkItem",
                column: "RequirePaintingListId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_StandradTimeExtId",
                table: "BlastWorkItem",
                column: "StandradTimeExtId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_StandradTimeIntId",
                table: "BlastWorkItem",
                column: "StandradTimeIntId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_SurfaceTypeExtId",
                table: "BlastWorkItem",
                column: "SurfaceTypeExtId");

            migrationBuilder.CreateIndex(
                name: "IX_BlastWorkItem_SurfaceTypeIntId",
                table: "BlastWorkItem",
                column: "SurfaceTypeIntId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_ExtColorItemId",
                table: "PaintWorkItem",
                column: "ExtColorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_IntColorItemId",
                table: "PaintWorkItem",
                column: "IntColorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_RequirePaintingListId",
                table: "PaintWorkItem",
                column: "RequirePaintingListId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_StandradTimeExtId",
                table: "PaintWorkItem",
                column: "StandradTimeExtId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintWorkItem_StandradTimeIntId",
                table: "PaintWorkItem",
                column: "StandradTimeIntId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlastWorkItem");

            migrationBuilder.DropTable(
                name: "PaintWorkItem");

            migrationBuilder.CreateTable(
                name: "RequirePaintingSub",
                columns: table => new
                {
                    RequirePaintingSubId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Area = table.Column<double>(nullable: true),
                    ColorItemId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    DFTMax = table.Column<double>(nullable: true),
                    DFTMin = table.Column<double>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintingArea = table.Column<int>(nullable: true),
                    PaintingType = table.Column<int>(nullable: true),
                    RequirePaintingListId = table.Column<int>(nullable: true),
                    StandradTimeId = table.Column<int>(nullable: true),
                    SurfaceTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingSub", x => x.RequirePaintingSubId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingSub_ColorItem_ColorItemId",
                        column: x => x.ColorItemId,
                        principalTable: "ColorItem",
                        principalColumn: "ColorItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequirePaintingSub_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequirePaintingSub_StandradTime_StandradTimeId",
                        column: x => x.StandradTimeId,
                        principalTable: "StandradTime",
                        principalColumn: "StandradTimeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequirePaintingSub_SurfaceType_SurfaceTypeId",
                        column: x => x.SurfaceTypeId,
                        principalTable: "SurfaceType",
                        principalColumn: "SurfaceTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingSub_ColorItemId",
                table: "RequirePaintingSub",
                column: "ColorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingSub_RequirePaintingListId",
                table: "RequirePaintingSub",
                column: "RequirePaintingListId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingSub_StandradTimeId",
                table: "RequirePaintingSub",
                column: "StandradTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingSub_SurfaceTypeId",
                table: "RequirePaintingSub",
                column: "SurfaceTypeId");
        }
    }
}
