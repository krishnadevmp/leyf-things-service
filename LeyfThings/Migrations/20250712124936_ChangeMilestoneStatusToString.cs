using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeyfThings.Migrations
{
    /// <inheritdoc />
    public partial class ChangeMilestoneStatusToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "MileStones");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MileStones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "MileStones");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "MileStones",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
