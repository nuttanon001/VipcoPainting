using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColorItem",
                columns: table => new
                {
                    ColorItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ColorCode = table.Column<string>(maxLength: 50, nullable: true),
                    ColorName = table.Column<string>(maxLength: 200, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    VolumeSolids = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorItem", x => x.ColorItemId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCodeMaster",
                columns: table => new
                {
                    ProjectCodeMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(nullable: true),
                    ProjectCode = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCodeMaster", x => x.ProjectCodeMasterId);
                });

            migrationBuilder.CreateTable(
                name: "StandradTime",
                columns: table => new
                {
                    StandradTimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PercentLoss = table.Column<double>(nullable: true),
                    Rate = table.Column<double>(nullable: true),
                    RateUnit = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandradTime", x => x.StandradTimeId);
                });

            migrationBuilder.CreateTable(
                name: "SurfaceType",
                columns: table => new
                {
                    SurfaceTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    SurfaceCode = table.Column<string>(maxLength: 100, nullable: true),
                    SurfaceName = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfaceType", x => x.SurfaceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectCodeSub",
                columns: table => new
                {
                    ProjectCodeSubId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    ProjectCodeMasterId = table.Column<int>(nullable: true),
                    ProjectSubParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCodeSub", x => x.ProjectCodeSubId);
                    table.ForeignKey(
                        name: "FK_ProjectCodeSub_ProjectCodeMaster_ProjectCodeMasterId",
                        column: x => x.ProjectCodeMasterId,
                        principalTable: "ProjectCodeMaster",
                        principalColumn: "ProjectCodeMasterId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectCodeSub_ProjectCodeSub_ProjectSubParentId",
                        column: x => x.ProjectSubParentId,
                        principalTable: "ProjectCodeSub",
                        principalColumn: "ProjectCodeSubId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequirePaintingMaster",
                columns: table => new
                {
                    RequirePaintingMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintingSchedule = table.Column<string>(maxLength: 200, nullable: true),
                    ProjectCodeSubId = table.Column<int>(nullable: true),
                    ReceiveDate = table.Column<DateTime>(nullable: true),
                    ReceiveEmp = table.Column<string>(nullable: true),
                    RequireDate = table.Column<DateTime>(nullable: true),
                    RequireEmp = table.Column<string>(nullable: true),
                    RequireNo = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingMaster", x => x.RequirePaintingMasterId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingMaster_ProjectCodeSub_ProjectCodeSubId",
                        column: x => x.ProjectCodeSubId,
                        principalTable: "ProjectCodeSub",
                        principalColumn: "ProjectCodeSubId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequirePaintingList",
                columns: table => new
                {
                    RequirePaintingListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 250, nullable: false),
                    DrawingNo = table.Column<string>(maxLength: 150, nullable: true),
                    FieldWeld = table.Column<bool>(nullable: true),
                    ITP = table.Column<bool>(nullable: true),
                    Insulation = table.Column<bool>(nullable: true),
                    MarkNo = table.Column<string>(maxLength: 150, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PartNo = table.Column<string>(maxLength: 150, nullable: true),
                    PlanEnd = table.Column<DateTime>(nullable: true),
                    PlanStart = table.Column<DateTime>(nullable: true),
                    Quantity = table.Column<double>(nullable: true),
                    RequirePaintingMasterId = table.Column<int>(nullable: true),
                    SizeH = table.Column<double>(nullable: true),
                    SizeL = table.Column<double>(nullable: true),
                    SizeW = table.Column<double>(nullable: true),
                    UnitNo = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingList", x => x.RequirePaintingListId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingList_RequirePaintingMaster_RequirePaintingMasterId",
                        column: x => x.RequirePaintingMasterId,
                        principalTable: "RequirePaintingMaster",
                        principalColumn: "RequirePaintingMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "IX_ProjectCodeSub_ProjectCodeMasterId",
                table: "ProjectCodeSub",
                column: "ProjectCodeMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCodeSub_ProjectSubParentId",
                table: "ProjectCodeSub",
                column: "ProjectSubParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingList_RequirePaintingMasterId",
                table: "RequirePaintingList",
                column: "RequirePaintingMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingMaster_ProjectCodeSubId",
                table: "RequirePaintingMaster",
                column: "ProjectCodeSubId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirePaintingSub");

            migrationBuilder.DropTable(
                name: "ColorItem");

            migrationBuilder.DropTable(
                name: "RequirePaintingList");

            migrationBuilder.DropTable(
                name: "StandradTime");

            migrationBuilder.DropTable(
                name: "SurfaceType");

            migrationBuilder.DropTable(
                name: "RequirePaintingMaster");

            migrationBuilder.DropTable(
                name: "ProjectCodeSub");

            migrationBuilder.DropTable(
                name: "ProjectCodeMaster");
        }
    }
}
