using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequirePaintingStatus",
                table: "RequirePaintingMaster",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaintTeam",
                columns: table => new
                {
                    PaintTeamId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    Ramark = table.Column<string>(maxLength: 200, nullable: true),
                    TeamName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintTeam", x => x.PaintTeamId);
                });

            migrationBuilder.CreateTable(
                name: "TaskMaster",
                columns: table => new
                {
                    TaskMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualEDate = table.Column<DateTime>(nullable: true),
                    ActualSDate = table.Column<DateTime>(nullable: true),
                    AssignBy = table.Column<string>(nullable: true),
                    AssignDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    RequirePaintingListId = table.Column<int>(nullable: true),
                    TaskNo = table.Column<string>(maxLength: 50, nullable: true),
                    TaskProgress = table.Column<double>(maxLength: 100, nullable: true),
                    TaskStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskMaster", x => x.TaskMasterId);
                    table.ForeignKey(
                        name: "FK_TaskMaster_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlastRoom",
                columns: table => new
                {
                    BlastRoomId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlastRoomName = table.Column<string>(maxLength: 100, nullable: true),
                    BlastRoomNumber = table.Column<int>(maxLength: 100, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTeamId = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlastRoom", x => x.BlastRoomId);
                    table.ForeignKey(
                        name: "FK_BlastRoom_PaintTeam_PaintTeamId",
                        column: x => x.PaintTeamId,
                        principalTable: "PaintTeam",
                        principalColumn: "PaintTeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskPaintDetail",
                columns: table => new
                {
                    TaskPaintDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTeamId = table.Column<int>(nullable: true),
                    PaintWorkItemId = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    TaskMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskPaintDetail", x => x.TaskPaintDetailId);
                    table.ForeignKey(
                        name: "FK_TaskPaintDetail_PaintTeam_PaintTeamId",
                        column: x => x.PaintTeamId,
                        principalTable: "PaintTeam",
                        principalColumn: "PaintTeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskPaintDetail_PaintWorkItem_PaintWorkItemId",
                        column: x => x.PaintWorkItemId,
                        principalTable: "PaintWorkItem",
                        principalColumn: "PaintWorkItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskPaintDetail_TaskMaster_TaskMasterId",
                        column: x => x.TaskMasterId,
                        principalTable: "TaskMaster",
                        principalColumn: "TaskMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TaskBlastDetail",
                columns: table => new
                {
                    TaskBlastDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BlastRoomId = table.Column<int>(nullable: true),
                    BlastWorkItemId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    TaskMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskBlastDetail", x => x.TaskBlastDetailId);
                    table.ForeignKey(
                        name: "FK_TaskBlastDetail_BlastRoom_BlastRoomId",
                        column: x => x.BlastRoomId,
                        principalTable: "BlastRoom",
                        principalColumn: "BlastRoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskBlastDetail_BlastWorkItem_BlastWorkItemId",
                        column: x => x.BlastWorkItemId,
                        principalTable: "BlastWorkItem",
                        principalColumn: "BlastWorkItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskBlastDetail_TaskMaster_TaskMasterId",
                        column: x => x.TaskMasterId,
                        principalTable: "TaskMaster",
                        principalColumn: "TaskMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequisitionMaster",
                columns: table => new
                {
                    RequisitionMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorItemId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    Quantity = table.Column<double>(nullable: true),
                    RequisitionBy = table.Column<string>(maxLength: 50, nullable: true),
                    RequisitionDate = table.Column<DateTime>(nullable: true),
                    TaskPaintDetailId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequisitionMaster", x => x.RequisitionMasterId);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_ColorItem_ColorItemId",
                        column: x => x.ColorItemId,
                        principalTable: "ColorItem",
                        principalColumn: "ColorItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequisitionMaster_TaskPaintDetail_TaskPaintDetailId",
                        column: x => x.TaskPaintDetailId,
                        principalTable: "TaskPaintDetail",
                        principalColumn: "TaskPaintDetailId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlastRoom_PaintTeamId",
                table: "BlastRoom",
                column: "PaintTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_ColorItemId",
                table: "RequisitionMaster",
                column: "ColorItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_TaskPaintDetailId",
                table: "RequisitionMaster",
                column: "TaskPaintDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBlastDetail_BlastRoomId",
                table: "TaskBlastDetail",
                column: "BlastRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBlastDetail_BlastWorkItemId",
                table: "TaskBlastDetail",
                column: "BlastWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskBlastDetail_TaskMasterId",
                table: "TaskBlastDetail",
                column: "TaskMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskMaster_RequirePaintingListId",
                table: "TaskMaster",
                column: "RequirePaintingListId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPaintDetail_PaintTeamId",
                table: "TaskPaintDetail",
                column: "PaintTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPaintDetail_PaintWorkItemId",
                table: "TaskPaintDetail",
                column: "PaintWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskPaintDetail_TaskMasterId",
                table: "TaskPaintDetail",
                column: "TaskMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequisitionMaster");

            migrationBuilder.DropTable(
                name: "TaskBlastDetail");

            migrationBuilder.DropTable(
                name: "TaskPaintDetail");

            migrationBuilder.DropTable(
                name: "BlastRoom");

            migrationBuilder.DropTable(
                name: "TaskMaster");

            migrationBuilder.DropTable(
                name: "PaintTeam");

            migrationBuilder.DropColumn(
                name: "RequirePaintingStatus",
                table: "RequirePaintingMaster");
        }
    }
}
