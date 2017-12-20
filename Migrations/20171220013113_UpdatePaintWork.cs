using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdatePaintWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "BlastWorkItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "BlastWorkItem",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "BlastWorkItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Modifyer",
                table: "BlastWorkItem",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "BlastWorkItem");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "BlastWorkItem");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "BlastWorkItem");

            migrationBuilder.DropColumn(
                name: "Modifyer",
                table: "BlastWorkItem");
        }
    }
}
