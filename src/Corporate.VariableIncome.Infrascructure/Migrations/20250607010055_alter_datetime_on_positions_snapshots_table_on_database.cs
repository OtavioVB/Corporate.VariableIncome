using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_datetime_on_positions_snapshots_table_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "snapshot_datetime",
                table: "position_snapshots",
                type: "TIMESTAMPTZ",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "snapshot_datetime",
                table: "position_snapshots",
                type: "TIMESTAMP",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMPTZ");
        }
    }
}
