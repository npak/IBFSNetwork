using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IBFSNetwork.Migrations
{
    public partial class UpdateFraudsterModel2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fraudsters_Alerts_AlertId",
                table: "Fraudsters");

            migrationBuilder.DropColumn(
                name: "isMain",
                table: "Fraudsters");

            migrationBuilder.CreateTable(
                name: "AlertFraudsters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AlertId = table.Column<int>(nullable: false),
                    FraudsterId = table.Column<int>(nullable: false),
                    isMain = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertFraudsters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlertFraudsters_Alerts_AlertId",
                        column: x => x.AlertId,
                        principalTable: "Alerts",
                        principalColumn: "AlertId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlertFraudsters_Fraudsters_FraudsterId",
                        column: x => x.FraudsterId,
                        principalTable: "Fraudsters",
                        principalColumn: "FraudsterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AlterColumn<int>(
                name: "AlertId",
                table: "Fraudsters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertFraudsters_AlertId",
                table: "AlertFraudsters",
                column: "AlertId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AlertFraudsters_FraudsterId",
                table: "AlertFraudsters",
                column: "FraudsterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fraudsters_Alerts_AlertId",
                table: "Fraudsters",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "AlertId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fraudsters_Alerts_AlertId",
                table: "Fraudsters");

            migrationBuilder.DropTable(
                name: "AlertFraudsters");

            migrationBuilder.AddColumn<bool>(
                name: "isMain",
                table: "Fraudsters",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "AlertId",
                table: "Fraudsters",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Fraudsters_Alerts_AlertId",
                table: "Fraudsters",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "AlertId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
