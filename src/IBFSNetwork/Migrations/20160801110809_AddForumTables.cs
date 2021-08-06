using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IBFSNetwork.Migrations
{
    public partial class AddForumTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainThread",
                columns: table => new
                {
                    MainThreadId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 511, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainThread", x => x.MainThreadId);
                });

            migrationBuilder.CreateTable(
                name: "SubThread",
                columns: table => new
                {
                    SubThreadId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    MainThreadId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 511, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubThread", x => x.SubThreadId);
                    table.ForeignKey(
                        name: "FK_SubThread_MainThread_MainThreadId",
                        column: x => x.MainThreadId,
                        principalTable: "MainThread",
                        principalColumn: "MainThreadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddColumn<int>(
                name: "SubThreadId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_SubThreadId",
                table: "Questions",
                column: "SubThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_SubThread_MainThreadId",
                table: "SubThread",
                column: "MainThreadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubThread_SubThreadId",
                table: "Questions",
                column: "SubThreadId",
                principalTable: "SubThread",
                principalColumn: "SubThreadId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubThread_SubThreadId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_SubThreadId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "SubThreadId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "SubThread");

            migrationBuilder.DropTable(
                name: "MainThread");
        }
    }
}
