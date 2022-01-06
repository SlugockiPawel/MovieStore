using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieStore.Data.Migrations
{
    public partial class MovieCollectionsDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollection_Collections_CollectionId",
                table: "MovieCollection");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollection_Movies_MovieId",
                table: "MovieCollection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCollection",
                table: "MovieCollection");

            migrationBuilder.RenameTable(
                name: "MovieCollection",
                newName: "MovieCollections");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCollection_MovieId",
                table: "MovieCollections",
                newName: "IX_MovieCollections_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCollection_CollectionId",
                table: "MovieCollections",
                newName: "IX_MovieCollections_CollectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCollections",
                table: "MovieCollections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollections_Collections_CollectionId",
                table: "MovieCollections",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollections_Movies_MovieId",
                table: "MovieCollections",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollections_Collections_CollectionId",
                table: "MovieCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCollections_Movies_MovieId",
                table: "MovieCollections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieCollections",
                table: "MovieCollections");

            migrationBuilder.RenameTable(
                name: "MovieCollections",
                newName: "MovieCollection");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCollections_MovieId",
                table: "MovieCollection",
                newName: "IX_MovieCollection_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_MovieCollections_CollectionId",
                table: "MovieCollection",
                newName: "IX_MovieCollection_CollectionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieCollection",
                table: "MovieCollection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollection_Collections_CollectionId",
                table: "MovieCollection",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCollection_Movies_MovieId",
                table: "MovieCollection",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
