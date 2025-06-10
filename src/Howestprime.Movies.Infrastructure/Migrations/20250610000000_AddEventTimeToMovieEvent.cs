using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Howestprime.Movies.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEventTimeToMovieEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the new EventTime column as nullable to match the entity
            migrationBuilder.AddColumn<DateTime>(
                name: "EventTime",
                table: "MovieEvents",
                type: "timestamp with time zone",
                nullable: true);

            // Update the EventTime column with combined Date and Time values
            migrationBuilder.Sql(@"
                UPDATE ""MovieEvents"" 
                SET ""EventTime"" = ""Date"" + ""Time""
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventTime",
                table: "MovieEvents");
        }
    }
}
