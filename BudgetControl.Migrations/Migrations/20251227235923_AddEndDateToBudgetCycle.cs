using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetControl.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddEndDateToBudgetCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "budget_cycles",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "budget_cycles");
        }
    }
}
