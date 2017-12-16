using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace VipcoPainting.Migrations
{
    public partial class RemoveProjectMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCodeSub_ProjectCodeMaster_ProjectCodeMasterId",
                table: "ProjectCodeSub");

            migrationBuilder.DropTable(
                name: "ProjectCodeMaster");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCodeSub_ProjectCodeMasterId",
                table: "ProjectCodeSub");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectCodeMaster",
                columns: table => new
                {
                    ProjectCodeMasterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateDate = table.Column<DateTime>(nullable: true),
                    Creator = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Modifyer = table.Column<string>(nullable: true),
                    ProjectCode = table.Column<string>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectCodeMaster", x => x.ProjectCodeMasterId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCodeSub_ProjectCodeMasterId",
                table: "ProjectCodeSub",
                column: "ProjectCodeMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCodeSub_ProjectCodeMaster_ProjectCodeMasterId",
                table: "ProjectCodeSub",
                column: "ProjectCodeMasterId",
                principalTable: "ProjectCodeMaster",
                principalColumn: "ProjectCodeMasterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
