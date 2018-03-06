using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class AddReqMasterAttach : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequirePaintingMasterHasAttach",
                columns: table => new
                {
                    RequirePaintingMasterHasAttachId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AttachFileId = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    RequirePaintingMasterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingMasterHasAttach", x => x.RequirePaintingMasterHasAttachId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingMasterHasAttach_RequirePaintingMaster_RequirePaintingMasterId",
                        column: x => x.RequirePaintingMasterId,
                        principalTable: "RequirePaintingMaster",
                        principalColumn: "RequirePaintingMasterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingMasterHasAttach_RequirePaintingMasterId",
                table: "RequirePaintingMasterHasAttach",
                column: "RequirePaintingMasterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirePaintingMasterHasAttach");
        }
    }
}
