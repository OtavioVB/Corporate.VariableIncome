using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class add_table_assets_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    idasset = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    code = table.Column<string>(type: "VARCHAR", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "VARCHAR", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asset_id", x => x.idasset);
                });

            migrationBuilder.CreateIndex(
                name: "uk_asset_code",
                table: "assets",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_asset_id",
                table: "assets",
                column: "idasset",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "assets");
        }
    }
}
