using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class chassiTofirstLast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NumeroChassiPrimeiraPeca",
                table: "MateriaisRecebidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroChassiUltimaPeca",
                table: "MateriaisRecebidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroChassiPrimeiraPeca",
                table: "MateriaisRecebidos");

            migrationBuilder.DropColumn(
                name: "NumeroChassiUltimaPeca",
                table: "MateriaisRecebidos");
        }
    }
}
