using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRancho.API.Migrations
{
    /// <inheritdoc />
    public partial class MovimientoPotrero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalMovimientoPotrero",
                columns: table => new
                {
                    AnimalMovimientoPotreroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    PotreroOrigenId = table.Column<int>(type: "int", nullable: true),
                    PotreroDestinoId = table.Column<int>(type: "int", nullable: false),
                    FechaMovimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalMovimientoPotrero", x => x.AnimalMovimientoPotreroId);
                    table.ForeignKey(
                        name: "FK_AnimalMovimientoPotrero_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalMovimientoPotrero_Potreros_PotreroDestinoId",
                        column: x => x.PotreroDestinoId,
                        principalTable: "Potreros",
                        principalColumn: "PotreroId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AnimalMovimientoPotrero_Potreros_PotreroOrigenId",
                        column: x => x.PotreroOrigenId,
                        principalTable: "Potreros",
                        principalColumn: "PotreroId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalMovimientoPotrero_AnimalId",
                table: "AnimalMovimientoPotrero",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalMovimientoPotrero_PotreroDestinoId",
                table: "AnimalMovimientoPotrero",
                column: "PotreroDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalMovimientoPotrero_PotreroOrigenId",
                table: "AnimalMovimientoPotrero",
                column: "PotreroOrigenId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalMovimientoPotrero");
        }
    }
}
