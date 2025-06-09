using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corporate.VariableIncome.Infrascructure.Migrations
{
    /// <inheritdoc />
    public partial class add_table_users_on_database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    iduser = table.Column<Guid>(type: "UUID", fixedLength: true, maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "VARCHAR", maxLength: 64, nullable: false),
                    brokerage_fee = table.Column<decimal>(type: "numeric(9,8)", nullable: false),
                    email = table.Column<string>(type: "VARCHAR", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_id", x => x.iduser);
                });

            migrationBuilder.CreateIndex(
                name: "uk_user_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uk_user_id",
                table: "users",
                column: "iduser",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
