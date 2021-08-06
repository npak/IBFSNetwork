using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class UdateForum20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies");

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies",
                column: "QuestionId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Questions");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies",
                column: "QuestionId");
        }
    }
}
