// Models/Book.cs
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        public DateTime PublicationDate { get; set; }
        public string Genre { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;

        // Navigation properties
        public List<BorrowingRecord> BorrowingRecords { get; set; } = new();
    }
}