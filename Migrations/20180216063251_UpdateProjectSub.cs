using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class UpdateProjectSub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequirePaintingListOption",
                columns: table => new
                {
                    RequirePaintingListOptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(maxLength: 50, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(maxLength: 50, nullable: true),
                    ReceiveWorkItem = table.Column<DateTime>(nullable: false),
                    RequirePaintingListId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequirePaintingListOption", x => x.RequirePaintingListOptionId);
                    table.ForeignKey(
                        name: "FK_RequirePaintingListOption_RequirePaintingList_RequirePaintingListId",
                        column: x => x.RequirePaintingListId,
                        principalTable: "RequirePaintingList",
                        principalColumn: "RequirePaintingListId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequirePaintingListOption_RequirePaintingListId",
                table: "RequirePaintingListOption",
                column: "RequirePaintingListId",
                unique: true,
                filter: "[RequirePaintingListId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequirePaintingListOption");
        }
    }
}
