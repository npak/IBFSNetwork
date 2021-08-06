using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IBFSNetwork.Migrations
{
    public partial class AddForumTablesBack1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubThread_MainThread_MainThreadId",
                table: "SubThread");

            migrationBuilder.DropIndex(
                name: "IX_SubThread_MainThreadId",
                table: "SubThread");

            migrationBuilder.DropColumn(
                name: "MainThreadId",
                table: "SubThread");

            migrationBuilder.DropTable(
                name: "MainThread");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "MainThreadId",
                table: "SubThread",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SubThread_MainThreadId",
                table: "SubThread",
                column: "MainThreadId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubThread_MainThread_MainThreadId",
                table: "SubThread",
                column: "MainThreadId",
                principalTable: "MainThread",
                principalColumn: "MainThreadId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
