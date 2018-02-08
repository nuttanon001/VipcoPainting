using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateStandTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AreaCodition",
                table: "StandradTime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Codition",
                table: "StandradTime",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendWorkItem",
                table: "RequirePaintingList",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaCodition",
                table: "StandradTime");

            migrationBuilder.DropColumn(
                name: "Codition",
                table: "StandradTime");

            migrationBuilder.DropColumn(
                name: "SendWorkItem",
                table: "RequirePaintingList");
        }
    }
}
