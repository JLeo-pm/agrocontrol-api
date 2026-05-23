using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRancho.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ranchos",
                columns: table => new
                {
                    RanchoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRancho = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Propietario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailContacto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranchos", x => x.RanchoId);
                });

            migrationBuilder.CreateTable(
                name: "Potreros",
                columns: table => new
                {
                    PotreroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RanchoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TamanoHectareas = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Potreros", x => x.PotreroId);
                    table.ForeignKey(
                        name: "FK_Potreros_Ranchos_RanchoId",
                        column: x => x.RanchoId,
                        principalTable: "Ranchos",
                        principalColumn: "RanchoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RanchoId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Ranchos_RanchoId",
                        column: x => x.RanchoId,
                        principalTable: "Ranchos",
                        principalColumn: "RanchoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    AnimalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RanchoId = table.Column<int>(type: "int", nullable: false),
                    PotreroId = table.Column<int>(type: "int", nullable: true),
                    NumeroArete = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Raza = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animales", x => x.AnimalId);
                    table.ForeignKey(
                        name: "FK_Animales_Potreros_PotreroId",
                        column: x => x.PotreroId,
                        principalTable: "Potreros",
                        principalColumn: "PotreroId");
                    table.ForeignKey(
                        name: "FK_Animales_Ranchos_RanchoId",
                        column: x => x.RanchoId,
                        principalTable: "Ranchos",
                        principalColumn: "RanchoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EstadoAnimal",
                columns: table => new
                {
                    EstadoAnimalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaEstado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecioVenta = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoAnimal", x => x.EstadoAnimalId);
                    table.ForeignKey(
                        name: "FK_EstadoAnimal_Animales_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animales",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animales_PotreroId",
                table: "Animales",
                column: "PotreroId");

            migrationBuilder.CreateIndex(
                name: "UX_Animal_Rancho_Arete",
                table: "Animales",
                columns: new[] { "RanchoId", "NumeroArete" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoAnimal_AnimalId",
                table: "EstadoAnimal",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Potreros_RanchoId",
                table: "Potreros",
                column: "RanchoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RanchoId",
                table: "Usuarios",
                column: "RanchoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoAnimal");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "Potreros");

            migrationBuilder.DropTable(
                name: "Ranchos");
        }
    }
}
