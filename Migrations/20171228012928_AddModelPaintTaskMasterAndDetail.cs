using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddModelPaintTaskMasterAndDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaintTaskDetailId",
                table: "RequisitionMaster",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaintTaskMaster",
                columns: table => new
                {
                    PaintTaskMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssignBy = table.Column<string>(nullable: true),
                    AssignDate = table.Column<DateTime>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    MainProgress = table.Column<double>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTaskStatus = table.Column<int>(nullable: false),
                    RequirePaintingListId = table.Column<int>(nullable: true),
                    TaskPaintNo = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintTaskMaster", x => x.PaintTaskMasterId);
                    table.ForeignKey(
                        name: "FK_PaintTaskMaster_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaintTaskDetail",
                columns: table => new
                {
                    PaintTaskDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActualEDate = table.Column<DateTime>(nullable: true),
                    ActualSDate = table.Column<DateTime>(nullable: true),
                    BlastRoomId = table.Column<int>(nullable: true),
                    BlastWorkItemId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTaskDetailLayer = table.Column<int>(nullable: true),
                    PaintTaskDetailStatus = table.Column<int>(nullable: true),
                    PaintTaskDetailType = table.Column<int>(nullable: true),
                    PaintTaskMasterId = table.Column<int>(nullable: true),
                    PaintTeamId = table.Column<int>(nullable: true),
                    PaintWorkItemId = table.Column<int>(nullable: true),
                    PlanEDate = table.Column<DateTime>(nullable: false),
                    PlanSDate = table.Column<DateTime>(nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    TaskDetailProgress = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaintTaskDetail", x => x.PaintTaskDetailId);
                    table.ForeignKey(
                        name: "FK_PaintTaskDetail_BlastRoom_BlastRoomId",
                        column: x => x.BlastRoomId,
                        principalTable: "BlastRoom",
                        principalColumn: "BlastRoomId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintTaskDetail_BlastWorkItem_BlastWorkItemId",
                        column: x => x.BlastWorkItemId,
                        principalTable: "BlastWorkItem",
                        principalColumn: "BlastWorkItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintTaskDetail_PaintTaskMaster_PaintTaskMasterId",
                        column: x => x.PaintTaskMasterId,
                        principalTable: "PaintTaskMaster",
                        principalColumn: "PaintTaskMasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintTaskDetail_PaintTeam_PaintTeamId",
                        column: x => x.PaintTeamId,
                        principalTable: "PaintTeam",
                        principalColumn: "PaintTeamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaintTaskDetail_PaintWorkItem_PaintWorkItemId",
                        column: x => x.PaintWorkItemId,
                        principalTable: "PaintWorkItem",
                        principalColumn: "PaintWorkItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequisitionMaster_PaintTaskDetailId",
                table: "RequisitionMaster",
                column: "PaintTaskDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_BlastRoomId",
                table: "PaintTaskDetail",
                column: "BlastRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_BlastWorkItemId",
                table: "PaintTaskDetail",
                column: "BlastWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_PaintTaskMasterId",
                table: "PaintTaskDetail",
                column: "PaintTaskMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_PaintTeamId",
                table: "PaintTaskDetail",
                column: "PaintTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_PaintWorkItemId",
                table: "PaintTaskDetail",
                column: "PaintWorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskMaster_RequirePaintingListId",
                table: "PaintTaskMaster",
                column: "RequirePaintingListId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequisitionMaster_PaintTaskDetail_PaintTaskDetailId",
                table: "RequisitionMaster",
                column: "PaintTaskDetailId",
                principalTable: "PaintTaskDetail",
                principalColumn: "PaintTaskDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequisitionMaster_PaintTaskDetail_PaintTaskDetailId",
                table: "RequisitionMaster");

            migrationBuilder.DropTable(
                name: "PaintTaskDetail");

            migrationBuilder.DropTable(
                name: "PaintTaskMaster");

            migrationBuilder.DropIndex(
                name: "IX_RequisitionMaster_PaintTaskDetailId",
                table: "RequisitionMaster");

            migrationBuilder.DropColumn(
                name: "PaintTaskDetailId",
                table: "RequisitionMaster");
        }
    }
}
