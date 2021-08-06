using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class addLastReplyId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastMessageId",
                table: "SubThreads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastMessageId",
                table: "RootForums",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastMessageId",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastMessageId",
                table: "Forums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "SubThreads");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "RootForums");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Forums");
        }
    }
}
