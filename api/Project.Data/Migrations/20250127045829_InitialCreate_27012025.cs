using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_27012025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Evaluate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Transaction",
                table: "Notifications");

            migrationBuilder.AddColumn<Guid>(
                name: "PersonInCharge",
                table: "TimeShifts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriorityEnum",
                table: "TimeShifts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Workflow",
                table: "TimeShifts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonInCharge",
                table: "TimeShifts");

            migrationBuilder.DropColumn(
                name: "PriorityEnum",
                table: "TimeShifts");

            migrationBuilder.DropColumn(
                name: "Workflow",
                table: "TimeShifts");

            migrationBuilder.AddColumn<int>(
                name: "Evaluate",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Transaction",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
