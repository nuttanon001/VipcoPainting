using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateSubPaymentV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubPaymentDetail_PaintTaskDetail_PaintTaskDetailId",
                table: "SubPaymentDetail");

            migrationBuilder.RenameColumn(
                name: "PaintTaskDetailId",
                table: "SubPaymentDetail",
                newName: "PaymentDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_SubPaymentDetail_PaintTaskDetailId",
                table: "SubPaymentDetail",
                newName: "IX_SubPaymentDetail_PaymentDetailId");

            migrationBuilder.AddColumn<int>(
                name: "PrecedingSubPaymentId",
                table: "SubPaymentMaster",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AdditionArea",
                table: "SubPaymentDetail",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AdditionCost",
                table: "SubPaymentDetail",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubPaymentMaster_PrecedingSubPaymentId",
                table: "SubPaymentMaster",
                column: "PrecedingSubPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubPaymentDetail_PaymentDetail_PaymentDetailId",
                table: "SubPaymentDetail",
                column: "PaymentDetailId",
                principalTable: "PaymentDetail",
                principalColumn: "PaymentDetailId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubPaymentMaster_SubPaymentMaster_PrecedingSubPaymentId",
                table: "SubPaymentMaster",
                column: "PrecedingSubPaymentId",
                principalTable: "SubPaymentMaster",
                principalColumn: "SubPaymentMasterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubPaymentDetail_PaymentDetail_PaymentDetailId",
                table: "SubPaymentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_SubPaymentMaster_SubPaymentMaster_PrecedingSubPaymentId",
                table: "SubPaymentMaster");

            migrationBuilder.DropIndex(
                name: "IX_SubPaymentMaster_PrecedingSubPaymentId",
                table: "SubPaymentMaster");

            migrationBuilder.DropColumn(
                name: "PrecedingSubPaymentId",
                table: "SubPaymentMaster");

            migrationBuilder.DropColumn(
                name: "AdditionArea",
                table: "SubPaymentDetail");

            migrationBuilder.DropColumn(
                name: "AdditionCost",
                table: "SubPaymentDetail");

            migrationBuilder.RenameColumn(
                name: "PaymentDetailId",
                table: "SubPaymentDetail",
                newName: "PaintTaskDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_SubPaymentDetail_PaymentDetailId",
                table: "SubPaymentDetail",
                newName: "IX_SubPaymentDetail_PaintTaskDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubPaymentDetail_PaintTaskDetail_PaintTaskDetailId",
                table: "SubPaymentDetail",
                column: "PaintTaskDetailId",
                principalTable: "PaintTaskDetail",
                principalColumn: "PaintTaskDetailId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
