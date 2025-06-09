using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class add_quotations_table_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "operations",
                type: "VARCHAR",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "quotations",
                columns: table => new
                {
                    idquotation = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    asset_id = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    unitary_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    quotation_date = table.Column<DateTime>(type: "TIMESTAMPTZ", fixedLength: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_quotation_id", x => x.idquotation);
                    table.ForeignKey(
                        name: "fk_quotation_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "idasset",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_quotation_asset_datetime",
                table: "quotations",
                columns: new[] { "asset_id", "quotation_date" });

            migrationBuilder.CreateIndex(
                name: "idx_quotation_datetime",
                table: "quotations",
                column: "quotation_date",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "uk_quotation_id",
                table: "quotations",
                column: "idquotation",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quotations");

            migrationBuilder.DropColumn(
                name: "type",
                table: "operations");
        }
    }
}
