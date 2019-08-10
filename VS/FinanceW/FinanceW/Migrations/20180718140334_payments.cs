using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FinanceW.Migrations
{
    public partial class payments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashIncome_Expense_ExpenseId",
                table: "CashIncome");

            migrationBuilder.DropIndex(
                name: "IX_CashIncome_ExpenseId",
                table: "CashIncome");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "CashIncome");

            migrationBuilder.CreateTable(
                name: "PayExpense",
                columns: table => new
                {
                    PayExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    PayExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    StatusPayExpense = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayExpense", x => x.PayExpenseId);
                    table.ForeignKey(
                        name: "FK_PayExpense_Expense_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expense",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayExpense_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayProduct",
                columns: table => new
                {
                    PayProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreditCardCutId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayProductDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductIdFrom = table.Column<int>(type: "int", nullable: false),
                    ProductIdTo = table.Column<int>(type: "int", nullable: false),
                    StatusPayProduct = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayProduct", x => x.PayProductId);
                    table.ForeignKey(
                        name: "FK_PayProduct_CreditCardCut_CreditCardCutId",
                        column: x => x.CreditCardCutId,
                        principalTable: "CreditCardCut",
                        principalColumn: "CreditCardCutId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayProduct_Product_ProductIdFrom",
                        column: x => x.ProductIdFrom,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PayProduct_Product_ProductIdTo",
                        column: x => x.ProductIdTo,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayExpense_ExpenseId",
                table: "PayExpense",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_PayExpense_ProductId",
                table: "PayExpense",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PayProduct_CreditCardCutId",
                table: "PayProduct",
                column: "CreditCardCutId");

            migrationBuilder.CreateIndex(
                name: "IX_PayProduct_ProductIdFrom",
                table: "PayProduct",
                column: "ProductIdFrom");

            migrationBuilder.CreateIndex(
                name: "IX_PayProduct_ProductIdTo",
                table: "PayProduct",
                column: "ProductIdTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayExpense");

            migrationBuilder.DropTable(
                name: "PayProduct");

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "CashIncome",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashIncome_ExpenseId",
                table: "CashIncome",
                column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashIncome_Expense_ExpenseId",
                table: "CashIncome",
                column: "ExpenseId",
                principalTable: "Expense",
                principalColumn: "ExpenseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
