using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class AddFKQuestionnaire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireAnswers_QuestionnaireModels_QuestionnaireModelId",
                table: "QuestionnaireAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionnaireModelId",
                table: "QuestionnaireAnswers",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireAnswers_QuestionnaireModels_QuestionnaireModelId",
                table: "QuestionnaireAnswers",
                column: "QuestionnaireModelId",
                principalTable: "QuestionnaireModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionnaireAnswers_QuestionnaireModels_QuestionnaireModelId",
                table: "QuestionnaireAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionnaireModelId",
                table: "QuestionnaireAnswers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionnaireAnswers_QuestionnaireModels_QuestionnaireModelId",
                table: "QuestionnaireAnswers",
                column: "QuestionnaireModelId",
                principalTable: "QuestionnaireModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
