using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IBFSNetwork.Migrations
{
    public partial class updateForumAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubThread_SubThreadId",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubThread",
                table: "SubThread");

            migrationBuilder.DropColumn(
                name: "ReplayCount",
                table: "Questions");

            migrationBuilder.CreateTable(
                name: "RootForums",
                columns: table => new
                {
                    RootForumId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 511, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RootForums", x => x.RootForumId);
                });

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    ForumId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    RootForumId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 511, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.ForumId);
                    table.ForeignKey(
                        name: "FK_Forums_RootForums_RootForumId",
                        column: x => x.RootForumId,
                        principalTable: "RootForums",
                        principalColumn: "RootForumId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<int>(
                name: "ForumId",
                table: "SubThread",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ForumId",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReplyCount",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubThreads",
                table: "SubThread",
                column: "SubThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_SubThreads_ForumId",
                table: "SubThread",
                column: "ForumId");

            migrationBuilder.AlterColumn<int>(
                name: "SubThreadId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_ForumId",
                table: "Questions",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_RootForumId",
                table: "Forums",
                column: "RootForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Forums_ForumId",
                table: "Questions",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "ForumId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubThreads_SubThreadId",
                table: "Questions",
                column: "SubThreadId",
                principalTable: "SubThread",
                principalColumn: "SubThreadId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SubThreads_Forums_ForumId",
                table: "SubThread",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "ForumId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameTable(
                name: "SubThread",
                newName: "SubThreads");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Forums_ForumId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_SubThreads_SubThreadId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_SubThreads_Forums_ForumId",
                table: "SubThreads");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubThreads",
                table: "SubThreads");

            migrationBuilder.DropIndex(
                name: "IX_SubThreads_ForumId",
                table: "SubThreads");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ForumId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "SubThreads");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ReplyCount",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropTable(
                name: "RootForums");

            migrationBuilder.AddColumn<int>(
                name: "ReplayCount",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubThread",
                table: "SubThreads",
                column: "SubThreadId");

            migrationBuilder.AlterColumn<int>(
                name: "SubThreadId",
                table: "Questions",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_SubThread_SubThreadId",
                table: "Questions",
                column: "SubThreadId",
                principalTable: "SubThreads",
                principalColumn: "SubThreadId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameTable(
                name: "SubThreads",
                newName: "SubThread");
        }
    }
}
