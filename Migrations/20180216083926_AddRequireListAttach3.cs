using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddRequireListAttach3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequirePaintingListHasAttach_AttachFile_AttachFileId",
                table: "RequirePaintingListHasAttach");

            migrationBuilder.DropTable(
                name: "AttachFile");

            migrationBuilder.DropIndex(
                name: "IX_RequirePaintingListHasAttach_AttachFileId",
                table: "RequirePaintingListHasAttach");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttachFile",
                columns: table => new
                {
                    AttachFileId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    FileAddress = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachFile", x => x.AttachFileId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingListHasAttach_AttachFileId",
                table: "RequirePaintingListHasAttach",
                column: "AttachFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequirePaintingListHasAttach_AttachFile_AttachFileId",
                table: "RequirePaintingListHasAttach",
                column: "AttachFileId",
                principalTable: "AttachFile",
                principalColumn: "AttachFileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
