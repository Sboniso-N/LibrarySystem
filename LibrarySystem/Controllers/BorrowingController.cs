// Controllers/BorrowingController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingController(LibraryDbContext context)
        {
            _context = context;
        }

        // POST: api/borrowing/borrow
        [HttpPost("borrow")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<BorrowingRecord>> BorrowBook(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);
            var member = await _context.Members.FindAsync(memberId);

            if (book == null || member == null)
            {
                return NotFound("Book or Member not found");
            }

            if (book.AvailableCopies <= 0)
            {
                return BadRequest("No copies available for borrowing");
            }

            var borrowingRecord = new BorrowingRecord
            {
                BookId = bookId,
                MemberId = memberId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14), // 2 weeks borrowing period
                Status = "Borrowed"
            };

            book.AvailableCopies--;
            if (book.AvailableCopies == 0)
            {
                book.IsAvailable = false;
            }

            _context.BorrowingRecords.Add(borrowingRecord);
            await _context.SaveChangesAsync();

            return Ok(borrowingRecord);
        }

        // POST: api/borrowing/return
        [HttpPost("return")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> ReturnBook(int recordId)
        {
            var record = await _context.BorrowingRecords
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == recordId);

            if (record == null)
            {
                return NotFound("Borrowing record not found");
            }

            if (record.Status == "Returned")
            {
                return BadRequest("Book already returned");
            }

            record.ReturnDate = DateTime.UtcNow;
            record.Status = "Returned";

            var book = record.Book;
            book.AvailableCopies++;
            book.IsAvailable = true;

            await _context.SaveChangesAsync();

            return Ok("Book returned successfully");
        }

        // GET: api/borrowing/member/5
        [HttpGet("member/{memberId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<IEnumerable<BorrowingRecord>>> GetMemberBorrowingHistory(int memberId)
        {
            var records = await _context.BorrowingRecords
                .Include(r => r.Book)
                .Where(r => r.MemberId == memberId)
                .ToListAsync();

            return records;
        }
    }
}