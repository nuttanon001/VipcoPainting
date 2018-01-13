using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddModelPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentDetailId",
                table: "PaintTaskDetail",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDetail",
                columns: table => new
                {
                    PaymentDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    LastCost = table.Column<double>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaymentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDetail", x => x.PaymentDetailId);
                });

            migrationBuilder.CreateTable(
                name: "SubPaymentMaster",
                columns: table => new
                {
                    SubPaymentMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    EmpApproved1 = table.Column<string>(maxLength: 50, nullable: true),
                    EmpApproved2 = table.Column<string>(maxLength: 50, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTeamId = table.Column<int>(nullable: true),
                    ProjectCodeMasterId = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SubPaymentDate = table.Column<DateTime>(nullable: true),
                    SubPaymentMasterStatus = table.Column<int>(nullable: true),
                    SubPaymentNo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubPaymentMaster", x => x.SubPaymentMasterId);
                    table.ForeignKey(
                        name: "FK_SubPaymentMaster_PaintTeam_PaintTeamId",
                        column: x => x.PaintTeamId,
                        principalTable: "PaintTeam",
                        principalColumn: "PaintTeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentCostHistory",
                columns: table => new
                {
                    PaymentCostHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaymentCost = table.Column<double>(nullable: false),
                    PaymentDetailId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCostHistory", x => x.PaymentCostHistoryId);
                    table.ForeignKey(
                        name: "FK_PaymentCostHistory_PaymentDetail_PaymentDetailId",
                        column: x => x.PaymentDetailId,
                        principalTable: "PaymentDetail",
                        principalColumn: "PaymentDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubPaymentDetail",
                columns: table => new
                {
                    SubPaymentDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AreaWorkLoad = table.Column<double>(nullable: true),
                    CalcCost = table.Column<double>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    PaintTaskDetailId = table.Column<int>(nullable: true),
                    PaymentCostHistoryId = table.Column<int>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    SubPaymentMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubPaymentDetail", x => x.SubPaymentDetailId);
                    table.ForeignKey(
                        name: "FK_SubPaymentDetail_PaintTaskDetail_PaintTaskDetailId",
                        column: x => x.PaintTaskDetailId,
                        principalTable: "PaintTaskDetail",
                        principalColumn: "PaintTaskDetailId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubPaymentDetail_PaymentCostHistory_PaymentCostHistoryId",
                        column: x => x.PaymentCostHistoryId,
                        principalTable: "PaymentCostHistory",
                        principalColumn: "PaymentCostHistoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubPaymentDetail_SubPaymentMaster_SubPaymentMasterId",
                        column: x => x.SubPaymentMasterId,
                        principalTable: "SubPaymentMaster",
                        principalColumn: "SubPaymentMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskDetail_PaymentDetailId",
                table: "PaintTaskDetail",
                column: "PaymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCostHistory_PaymentDetailId",
                table: "PaymentCostHistory",
                column: "PaymentDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPaymentDetail_PaintTaskDetailId",
                table: "SubPaymentDetail",
                column: "PaintTaskDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPaymentDetail_PaymentCostHistoryId",
                table: "SubPaymentDetail",
                column: "PaymentCostHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPaymentDetail_SubPaymentMasterId",
                table: "SubPaymentDetail",
                column: "SubPaymentMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SubPaymentMaster_PaintTeamId",
                table: "SubPaymentMaster",
                column: "PaintTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaintTaskDetail_PaymentDetail_PaymentDetailId",
                table: "PaintTaskDetail",
                column: "PaymentDetailId",
                principalTable: "PaymentDetail",
                principalColumn: "PaymentDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaintTaskDetail_PaymentDetail_PaymentDetailId",
                table: "PaintTaskDetail");

            migrationBuilder.DropTable(
                name: "SubPaymentDetail");

            migrationBuilder.DropTable(
                name: "PaymentCostHistory");

            migrationBuilder.DropTable(
                name: "SubPaymentMaster");

            migrationBuilder.DropTable(
                name: "PaymentDetail");

            migrationBuilder.DropIndex(
                name: "IX_PaintTaskDetail_PaymentDetailId",
                table: "PaintTaskDetail");

            migrationBuilder.DropColumn(
                name: "PaymentDetailId",
                table: "PaintTaskDetail");
        }
    }
}
