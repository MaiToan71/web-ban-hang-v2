using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_workrecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WorkRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgUrlCheckin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgUrlCheckout = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOnly = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromWorkDateRegis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToWorkDateRegis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromWorkDateEffect = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToWorkDateEffect = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalHours = table.Column<double>(type: "float", nullable: true),
                    WorkType = table.Column<int>(type: "int", nullable: false),
                    Multiplier = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkRecordStatus = table.Column<int>(type: "int", nullable: true),
                    CreatedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkRecords", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppUsers");
        }
    }
}
