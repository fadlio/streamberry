using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streamberry.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieProvider",
                columns: table => new
                {
                    MoviesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProvidersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProvider", x => new { x.MoviesId, x.ProvidersId });
                    table.ForeignKey(
                        name: "FK_MovieProvider_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProvider_Provider_ProvidersId",
                        column: x => x.ProvidersId,
                        principalTable: "Provider",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieProvider_ProvidersId",
                table: "MovieProvider",
                column: "ProvidersId");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_Name",
                table: "Provider",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieProvider");

            migrationBuilder.DropTable(
                name: "Provider");
        }
    }
}
