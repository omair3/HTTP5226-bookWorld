using Microsoft.EntityFrameworkCore;

namespace BookWorld.Data
{
    public class BookWorldContext : DbContext
    {
        public BookWorldContext(DbContextOptions<BookWorldContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Book> Books { get; set; }
        public DbSet<Models.Author> Authors { get; set; }
        public DbSet<Models.Genre> Genres { get; set; }
        public DbSet<Models.BookGenre> BookGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            modelBuilder.Entity<Models.BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<Models.BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);
        }
    }
}