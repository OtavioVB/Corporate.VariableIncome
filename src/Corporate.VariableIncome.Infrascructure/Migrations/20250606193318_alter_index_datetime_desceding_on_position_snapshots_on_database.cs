using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_index_datetime_desceding_on_position_snapshots_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_position_snapshot_asset_user",
                table: "position_snapshots");

            migrationBuilder.RenameIndex(
                name: "IX_position_snapshots_user_id",
                table: "position_snapshots",
                newName: "idx_position_snapshot_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_position_snapshot_asset_id",
                table: "position_snapshots",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "idx_position_snapshot_datetime",
                table: "position_snapshots",
                column: "snapshot_datetime",
                descending: [true]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_position_snapshot_asset_id",
                table: "position_snapshots");

            migrationBuilder.DropIndex(
                name: "idx_position_snapshot_datetime",
                table: "position_snapshots");

            migrationBuilder.RenameIndex(
                name: "idx_position_snapshot_user_id",
                table: "position_snapshots",
                newName: "IX_position_snapshots_user_id");

            migrationBuilder.CreateIndex(
                name: "idx_position_snapshot_asset_user",
                table: "position_snapshots",
                columns: new[] { "asset_id", "user_id" });
        }
    }
}
