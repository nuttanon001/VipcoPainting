using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class ModifyStandardTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkStandardTimeId",
                table: "StandradTime",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StandradTime_LinkStandardTimeId",
                table: "StandradTime",
                column: "LinkStandardTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StandradTime_StandradTime_LinkStandardTimeId",
                table: "StandradTime",
                column: "LinkStandardTimeId",
                principalTable: "StandradTime",
                principalColumn: "StandradTimeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StandradTime_StandradTime_LinkStandardTimeId",
                table: "StandradTime");

            migrationBuilder.DropIndex(
                name: "IX_StandradTime_LinkStandardTimeId",
                table: "StandradTime");

            migrationBuilder.DropColumn(
                name: "LinkStandardTimeId",
                table: "StandradTime");
        }
    }
}
