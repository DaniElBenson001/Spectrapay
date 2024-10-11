using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Spectrapay.Services.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdNotfication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Notifications",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Notifications",
                nullable: false);
        }
    }
}
