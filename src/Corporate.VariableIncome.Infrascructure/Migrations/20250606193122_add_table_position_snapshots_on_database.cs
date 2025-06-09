using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class add_table_position_snapshots_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "position_snapshots",
                columns: table => new
                {
                    idpositionsnapshot = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    asset_id = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    user_id = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    average_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    snapshot_datetime = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    proft_and_loss = table.Column<decimal>(type: "numeric(20,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_position_snapshot_id", x => x.idpositionsnapshot);
                    table.ForeignKey(
                        name: "fk_position_snapshot_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "idasset",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_position_snapshot_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "iduser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_position_snapshot_asset_user",
                table: "position_snapshots",
                columns: new[] { "asset_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "IX_position_snapshots_user_id",
                table: "position_snapshots",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_position_snapshot_id",
                table: "position_snapshots",
                column: "idpositionsnapshot",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "position_snapshots");
        }
    }
}
