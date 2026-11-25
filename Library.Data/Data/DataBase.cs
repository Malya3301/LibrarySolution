using Library.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Data
{
    public class DataBase : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Issuance> Issuances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=LibraryDb;Username=postgres;Password=12345678");
        }

    }
}