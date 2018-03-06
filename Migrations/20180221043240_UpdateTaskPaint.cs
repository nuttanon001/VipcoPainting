using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateTaskPaint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaintTaskMaster_RequirePaintingListId",
                table: "PaintTaskMaster");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskMaster_RequirePaintingListId",
                table: "PaintTaskMaster",
                column: "RequirePaintingListId",
                unique: true,
                filter: "[RequirePaintingListId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PaintTaskMaster_RequirePaintingListId",
                table: "PaintTaskMaster");

            migrationBuilder.CreateIndex(
                name: "IX_PaintTaskMaster_RequirePaintingListId",
                table: "PaintTaskMaster",
                column: "RequirePaintingListId");
        }
    }
}
