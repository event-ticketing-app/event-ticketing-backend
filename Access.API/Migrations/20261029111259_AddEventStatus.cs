using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Access.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Events that already existed were public, so keep them visible (1 = Active).
            // New events start as Pending (0) and need admin approval.
            migrationBuilder.Sql("UPDATE Events SET Status = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");
        }
    }
}
