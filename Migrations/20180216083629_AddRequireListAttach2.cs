using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddRequireListAttach2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "RequirePaintingListHasAttach",
                columns: table => new
                {
                    RequirePaintingListHasAttachId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttachFileId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    RequirePaintingListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingListHasAttach", x => x.RequirePaintingListHasAttachId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingListHasAttach_AttachFile_AttachFileId",
                        column: x => x.AttachFileId,
                        principalTable: "AttachFile",
                        principalColumn: "AttachFileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequirePaintingListHasAttach_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingListHasAttach_AttachFileId",
                table: "RequirePaintingListHasAttach",
                column: "AttachFileId");

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingListHasAttach_RequirePaintingListId",
                table: "RequirePaintingListHasAttach",
                column: "RequirePaintingListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirePaintingListHasAttach");

            migrationBuilder.DropTable(
                name: "AttachFile");
        }
    }
}
