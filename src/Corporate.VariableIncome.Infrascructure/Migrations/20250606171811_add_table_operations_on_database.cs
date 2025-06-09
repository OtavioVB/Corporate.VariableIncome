using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class add_table_operations_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    idoperation = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    unitary_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    brokerage_fee = table.Column<decimal>(type: "numeric(3,2)", nullable: false),
                    asset_id = table.Column<Guid>(type: "UUID", maxLength: 36, nullable: false),
                    user_id = table.Column<Guid>(type: "UUID", maxLength: 36, nullable: false),
                    operation_date = table.Column<DateTime>(type: "TIMESTAMPTZ", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operation_id", x => x.idoperation);
                    table.ForeignKey(
                        name: "fk_operation_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "idasset",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_operation_user_id",
                        column: x => x.asset_id,
                        principalTable: "users",
                        principalColumn: "iduser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_operation_asset_id",
                table: "operations",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "idx_operation_date_time",
                table: "operations",
                column: "operation_date",
                descending: [true]);

            migrationBuilder.CreateIndex(
                name: "idx_operation_user_id",
                table: "operations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_operation_id",
                table: "operations",
                column: "idoperation",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "operations");
        }
    }
}
