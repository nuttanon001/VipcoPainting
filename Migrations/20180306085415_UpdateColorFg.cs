using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateColorFg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "FinishedGoodsMaster",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNumber",
                table: "FinishedGoodsMaster",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectCodeMasterId",
                table: "FinishedGoodsMaster",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "FinishedGoodsMaster");

            migrationBuilder.DropColumn(
                name: "LotNumber",
                table: "FinishedGoodsMaster");

            migrationBuilder.DropColumn(
                name: "ProjectCodeMasterId",
                table: "FinishedGoodsMaster");
        }
    }
}
