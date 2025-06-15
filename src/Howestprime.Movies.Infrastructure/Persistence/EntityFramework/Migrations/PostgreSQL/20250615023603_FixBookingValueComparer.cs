using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Howestprime.Movies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingValueComparer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MovieEvents_MovieEventId",
                table: "Bookings");


            migrationBuilder.AlterColumn<string>(
                name: "MovieEventId",
                table: "Bookings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MovieEvents_MovieEventId",
                table: "Bookings",
                column: "MovieEventId",
                principalTable: "MovieEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_MovieEvents_MovieEventId",
                table: "Bookings");


            migrationBuilder.AlterColumn<string>(
                name: "MovieEventId",
                table: "Bookings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_MovieEvents_MovieEventId",
                table: "Bookings",
                column: "MovieEventId",
                principalTable: "MovieEvents",
                principalColumn: "Id");
        }
    }
}
