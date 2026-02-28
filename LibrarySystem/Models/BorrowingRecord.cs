// Models/BorrowingRecord.cs
namespace LibrarySystem.Models
{
    public class BorrowingRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Borrowed"; // Borrowed, Returned, Overdue

        // Navigation properties
        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;
    }
}