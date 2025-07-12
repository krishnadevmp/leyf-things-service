using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeyfThings.Migrations
{
    /// <inheritdoc />
    public partial class DeleteProgressFromGoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Progress",
                table: "Goals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Progress",
                table: "Goals",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
