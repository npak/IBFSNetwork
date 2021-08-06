using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class UdateQuestionReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Questions_QuestionId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "ReplyId",
                table: "Questions");

            migrationBuilder.AlterColumn<string>(
                name: "ReplyMsg",
                table: "Replies",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Replies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Questions_QuestionId",
                table: "Replies",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Replies_Questions_QuestionId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies");

            migrationBuilder.AddColumn<int>(
                name: "ReplyId",
                table: "Questions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReplyMsg",
                table: "Replies",
                maxLength: 1023,
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Replies",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Replies_QuestionId",
                table: "Replies",
                column: "QuestionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_Questions_QuestionId",
                table: "Replies",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
