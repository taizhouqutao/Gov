using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    public partial class CarouselsAddPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsPublic",
                table: "Carousels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublicTime",
                table: "Carousels",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublicUserId",
                table: "Carousels",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Carousels");

            migrationBuilder.DropColumn(
                name: "PublicTime",
                table: "Carousels");

            migrationBuilder.DropColumn(
                name: "PublicUserId",
                table: "Carousels");
        }
    }
}
