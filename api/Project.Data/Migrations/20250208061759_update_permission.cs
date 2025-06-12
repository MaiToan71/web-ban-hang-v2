using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "TimeShiftTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "TimeShiftTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PersonInCharge",
                table: "TimeShiftTypes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriorityEnum",
                table: "TimeShiftTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "TimeShiftTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeShiftTypeId",
                table: "TimeShiftTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Workflow",
                table: "TimeShiftTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Component",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermissionType",
                table: "Permissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Route",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Code",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "PersonInCharge",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "PriorityEnum",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "TimeShiftTypeId",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "Workflow",
                table: "TimeShiftTypes");

            migrationBuilder.DropColumn(
                name: "Component",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "PermissionType",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Route",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AppUsers");
        }
    }
}
