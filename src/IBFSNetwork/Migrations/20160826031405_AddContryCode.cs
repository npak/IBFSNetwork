using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IBFSNetwork.Migrations
{
    public partial class AddContryCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "LocationStates",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Countries",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "LocationStates");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Countries");
        }
    }
}
