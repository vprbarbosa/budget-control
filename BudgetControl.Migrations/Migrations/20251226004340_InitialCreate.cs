using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetControl.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "funding_sources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funding_sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "spending_categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spending_categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "budget_cycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FundingSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    estimated_duration_days = table.Column<int>(type: "integer", nullable: false),
                    total_capacity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_budget_cycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_budget_cycles_funding_sources_FundingSourceId",
                        column: x => x.FundingSourceId,
                        principalTable: "funding_sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "day_allocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_closed = table.Column<bool>(type: "boolean", nullable: false),
                    BudgetCycleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_day_allocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_day_allocations_budget_cycles_BudgetCycleId",
                        column: x => x.BudgetCycleId,
                        principalTable: "budget_cycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partial_expenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    SpendingCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(600)", maxLength: 600, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    DayAllocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_partial_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_partial_expenses_day_allocations_DayAllocationId",
                        column: x => x.DayAllocationId,
                        principalTable: "day_allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_partial_expenses_spending_categories_SpendingCategoryId",
                        column: x => x.SpendingCategoryId,
                        principalTable: "spending_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_budget_cycles_FundingSourceId",
                table: "budget_cycles",
                column: "FundingSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_day_allocations_BudgetCycleId",
                table: "day_allocations",
                column: "BudgetCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_partial_expenses_DayAllocationId",
                table: "partial_expenses",
                column: "DayAllocationId");

            migrationBuilder.CreateIndex(
                name: "IX_partial_expenses_SpendingCategoryId",
                table: "partial_expenses",
                column: "SpendingCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partial_expenses");

            migrationBuilder.DropTable(
                name: "day_allocations");

            migrationBuilder.DropTable(
                name: "spending_categories");

            migrationBuilder.DropTable(
                name: "budget_cycles");

            migrationBuilder.DropTable(
                name: "funding_sources");
        }
    }
}
