using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotePool.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Notes_NoteId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_NoteId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "NoteId",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NoteId",
                table: "Files",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_NoteId",
                table: "Files",
                column: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Notes_NoteId",
                table: "Files",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
