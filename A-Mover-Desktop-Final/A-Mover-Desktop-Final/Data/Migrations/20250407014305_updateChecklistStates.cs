using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace A_Mover_Desktop_Final.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateChecklistStates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ChecklistMontagem");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ChecklistEmbalagem");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "ChecklistControlo");

            migrationBuilder.AddColumn<int>(
                name: "Verificado",
                table: "ChecklistMontagem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Incluido",
                table: "ChecklistEmbalagem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ControloFinal",
                table: "ChecklistControlo",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verificado",
                table: "ChecklistMontagem");

            migrationBuilder.DropColumn(
                name: "Incluido",
                table: "ChecklistEmbalagem");

            migrationBuilder.DropColumn(
                name: "ControloFinal",
                table: "ChecklistControlo");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ChecklistMontagem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ChecklistEmbalagem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "ChecklistControlo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
