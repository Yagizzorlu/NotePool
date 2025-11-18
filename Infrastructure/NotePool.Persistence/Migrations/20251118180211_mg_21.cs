using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotePool.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mg_21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NoteDownloads_UserId",
                table: "NoteDownloads");

            migrationBuilder.AddColumn<Guid>(
                name: "NoteId1",
                table: "NoteDownloads",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NoteDownloads_NoteId1",
                table: "NoteDownloads",
                column: "NoteId1");

            migrationBuilder.CreateIndex(
                name: "IX_NoteDownloads_UserId_NoteId",
                table: "NoteDownloads",
                columns: new[] { "UserId", "NoteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ParentId",
                table: "Comments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments",
                column: "ParentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteDownloads_Notes_NoteId1",
                table: "NoteDownloads",
                column: "NoteId1",
                principalTable: "Notes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteDownloads_Notes_NoteId1",
                table: "NoteDownloads");

            migrationBuilder.DropIndex(
                name: "IX_NoteDownloads_NoteId1",
                table: "NoteDownloads");

            migrationBuilder.DropIndex(
                name: "IX_NoteDownloads_UserId_NoteId",
                table: "NoteDownloads");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ParentId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "NoteId1",
                table: "NoteDownloads");

            migrationBuilder.CreateIndex(
                name: "IX_NoteDownloads_UserId",
                table: "NoteDownloads",
                column: "UserId");
        }
    }
}
