using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_decimal_on_all_tables_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "proft_and_loss",
                table: "position_snapshots",
                type: "numeric(20,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "average_price",
                table: "position_snapshots",
                type: "numeric(16,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "unitary_price",
                table: "operations",
                type: "numeric(16,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "brokerage_fee",
                table: "operations",
                type: "numeric(20,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(3,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "proft_and_loss",
                table: "position_snapshots",
                type: "numeric(20,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "average_price",
                table: "position_snapshots",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "unitary_price",
                table: "operations",
                type: "numeric(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(16,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "brokerage_fee",
                table: "operations",
                type: "numeric(3,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,6)");
        }
    }
}
