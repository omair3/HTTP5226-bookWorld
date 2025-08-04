using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookWorld.Migrations
{
    /// <inheritdoc />
    public partial class InitialSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "United Kingdom", "Jane Austen" },
                    { 2, "United Kingdom", "George Orwell" },
                    { 3, "United Kingdom", "Agatha Christie" },
                    { 4, "United Kingdom", "J.K. Rowling" },
                    { 5, "United States", "Ernest Hemingway" },
                    { 6, "United States", "Stephen King" },
                    { 7, "United States", "Toni Morrison" },
                    { 8, "United States", "Mark Twain" },
                    { 9, "United Kingdom", "Virginia Woolf" },
                    { 10, "United States", "Isaac Asimov" },
                    { 11, "United Kingdom", "Mary Shelley" },
                    { 12, "United Kingdom", "Charles Dickens" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Non-Fiction" },
                    { 3, "Mystery" },
                    { 4, "Fantasy" },
                    { 5, "Science Fiction" },
                    { 6, "Romance" },
                    { 7, "Historical" },
                    { 8, "Biography" },
                    { 9, "Thriller" },
                    { 10, "Adventure" },
                    { 11, "Horror" },
                    { 12, "Self-Help" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Pride and Prejudice" },
                    { 2, 2, "1984" },
                    { 3, 3, "Murder on the Orient Express" },
                    { 4, 4, "Harry Potter and the Philosopher's Stone" },
                    { 5, 5, "The Old Man and the Sea" },
                    { 6, 6, "The Shining" },
                    { 7, 7, "Beloved" },
                    { 8, 8, "The Adventures of Tom Sawyer" },
                    { 9, 9, "Mrs. Dalloway" },
                    { 10, 10, "Foundation" },
                    { 11, 11, "Frankenstein" },
                    { 12, 12, "Great Expectations" }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 4, 4 },
                    { 5, 7 },
                    { 6, 11 },
                    { 7, 8 },
                    { 8, 10 },
                    { 9, 1 },
                    { 10, 5 },
                    { 11, 11 },
                    { 12, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 5, 7 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 6, 11 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 7, 8 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 8, 10 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 10, 5 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 11, 11 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumns: new[] { "BookId", "GenreId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
