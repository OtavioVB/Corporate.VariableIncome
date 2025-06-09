using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class alter_fk_user_id_on_operations_table_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_operation_user_id",
                table: "operations");

            migrationBuilder.AddForeignKey(
                name: "fk_operation_user_id",
                table: "operations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "iduser",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_operation_user_id",
                table: "operations");

            migrationBuilder.AddForeignKey(
                name: "fk_operation_user_id",
                table: "operations",
                column: "asset_id",
                principalTable: "users",
                principalColumn: "iduser",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
