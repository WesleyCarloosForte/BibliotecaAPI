using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiBiblioteca.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenerosLibros");

            migrationBuilder.AddColumn<int>(
                name: "GeneroId",
                table: "Libros",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_GeneroId",
                table: "Libros",
                column: "GeneroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Libros_Generos_GeneroId",
                table: "Libros",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libros_Generos_GeneroId",
                table: "Libros");

            migrationBuilder.DropIndex(
                name: "IX_Libros_GeneroId",
                table: "Libros");

            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "Libros");

            migrationBuilder.CreateTable(
                name: "GenerosLibros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GeneroId = table.Column<int>(type: "INTEGER", nullable: false),
                    LibroId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenerosLibros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GenerosLibros_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenerosLibros_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenerosLibros_GeneroId",
                table: "GenerosLibros",
                column: "GeneroId");

            migrationBuilder.CreateIndex(
                name: "IX_GenerosLibros_LibroId",
                table: "GenerosLibros",
                column: "LibroId");
        }
    }
}
