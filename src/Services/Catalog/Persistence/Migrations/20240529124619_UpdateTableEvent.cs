using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Events_OrganizationInfoId",
                table: "Events",
                column: "OrganizationInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_OrganizationInfo_OrganizationInfoId",
                table: "Events",
                column: "OrganizationInfoId",
                principalTable: "OrganizationInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_OrganizationInfo_OrganizationInfoId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_OrganizationInfoId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tickets");
        }
    }
}
