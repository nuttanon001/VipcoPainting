using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class RemoveTaskMasterTaskPaintTaskBlast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskBlastDetail");

            migrationBuilder.DropTable(
                name: "TaskPaintDetail");

            migrationBuilder.DropTable(
                name: "TaskMaster");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                    TaskProgress = table.Column<double>(nullable: true),
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
    }
}
