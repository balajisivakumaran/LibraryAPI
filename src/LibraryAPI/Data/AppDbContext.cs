using LibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Data;

public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasData(
                new Book {
                    Id = 1,
                    Title = "The Lord of the Rings",
                    Author = "J.R.R. Tolkien",
                    YearofPublish = 1955
                },
                
                new Book {
                    Id = 2,
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Author = "J.K. Rowling",
                    YearofPublish = 1997
                },

                new Book {
                    Id = 3,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    YearofPublish = 1925
                }

            );
        }

    }