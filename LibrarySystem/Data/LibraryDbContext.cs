// Data/LibraryDbContext.cs
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Models;

namespace LibrarySystem.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) :
            base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "9780743273565", PublicationDate = new DateTime(1925, 4, 10), Genre = "Fiction", IsAvailable = true, TotalCopies = 3, AvailableCopies = 2 },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "9780061120084", PublicationDate = new DateTime(1960, 7, 11), Genre = "Fiction", IsAvailable = true, TotalCopies = 2, AvailableCopies = 1 }
            );

            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", PhoneNumber = "123-456-7890", MembershipDate = DateTime.UtcNow.AddMonths(-6), IsActive = true },
                new Member { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", PhoneNumber = "123-456-7891", MembershipDate = DateTime.UtcNow.AddMonths(-3), IsActive = true }
            );
        }
    }
}